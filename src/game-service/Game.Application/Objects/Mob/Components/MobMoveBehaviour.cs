using System;
using System.Collections;
using System.Timers;
using Coroutines;
using Game.Application.Components;
using Game.Messages;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    public class MobMoveBehaviour : ComponentBase, IMobMoveBehaviour
    {
        private readonly Random random = new();
        private readonly Timer positionSenderTimer;

        private IGameObject mob;
        private IProximityChecker proximityChecker;
        private IMobConfigDataProvider mobConfigDataProvider;
        private ICoroutineRunner coroutineRunner;

        private float direction;
        private bool isMoveStopped;

        public MobMoveBehaviour()
        {
            positionSenderTimer = new Timer
            {
                Interval = 100,
                AutoReset = true,
                Enabled = true
            };
            positionSenderTimer.Elapsed += (_, _) => SendPositionMessage();
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            mobConfigDataProvider = Components.Get<IMobConfigDataProvider>();
            mob = Components.Get<IGameObjectGetter>().Get();
        }

        protected override void OnRemoved()
        {
            Stop();

            positionSenderTimer?.Stop();
            positionSenderTimer?.Dispose();
        }

        public void Start()
        {
            if (coroutineRunner == null)
            {
                coroutineRunner = GetCoroutineRunner();
            }

            isMoveStopped = false;

            coroutineRunner?.Run(Move());
        }

        public void Stop()
        {
            isMoveStopped = true;

            coroutineRunner?.Stop(Move());
        }

        private IEnumerator Move()
        {
            var startPosition = mob.Transform.Position;
            var position = mob.Transform.Position;
            var mobConfigData = mobConfigDataProvider.Provide();
            var speed = mobConfigData.Speed;
            var distance = mobConfigData.Distance;

            direction = GetRandomDirection();

            while (!isMoveStopped)
            {
                var currentPosition = mob.Transform.Position;

                if (Vector2.Distance(startPosition, currentPosition) >= distance)
                {
                    direction *= -1;
                }

                position += new Vector2(direction, 0) * speed;

                mob.Transform.SetPosition(position);
                yield return null;
            }
        }

        private void SendPositionMessage()
        {
            var nearbyGameObjects = proximityChecker?.GetNearbyGameObjects();

            foreach (var gameObject in nearbyGameObjects)
            {
                var message = new PositionChangedMessage()
                {
                    GameObjectId = mob.Id,
                    X = mob.Transform.Position.X,
                    Y = mob.Transform.Position.Y
                };
                var messageSender = gameObject.Components.Get<IMessageSender>();

                messageSender?.SendMessage((byte)MessageCodes.PositionChanged, message);
            }
        }

        private float GetRandomDirection()
        {
            return random.Next(-1, 1) == 0 ? 0.1f : -0.1f;
        }

        private ICoroutineRunner GetCoroutineRunner()
        {
            var presenceSceneProvider =
                Components.Get<IPresenceSceneProvider>();
            var gameScene =
                presenceSceneProvider.GetScene();
            var gamePhysicsCreator =
                gameScene.Components.Get<IScenePhysicsCreator>();
            var physicsExecutor =
                gamePhysicsCreator.GetPhysicsExecutor();

            return physicsExecutor.GetCoroutineRunner();
        }
    }
}