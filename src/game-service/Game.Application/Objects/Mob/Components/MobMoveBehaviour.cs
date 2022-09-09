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
        private readonly Timer positionSenderTimer;
        private readonly Random random = new();

        private IGameObject mob;
        private IProximityChecker proximityChecker;
        private ICoroutineRunner coroutineRunner;
        private IMobConfigDataProvider mobConfigDataProvider;

        private bool isMoveStopped;

        public MobMoveBehaviour()
        {
            positionSenderTimer = new Timer(100);
            positionSenderTimer.Elapsed += (s, e) => SendPosition();
        }

        private void StartPositionSenderTimer()
        {
            positionSenderTimer.Start();
        }

        private void StopPositionSenderTimer()
        {
            positionSenderTimer.Stop();
        }

        public void Start()
        {
            var presenceSceneProvider =
                Components.Get<IPresenceSceneProvider>();
            var gameScene =
                presenceSceneProvider.GetScene();
            var gamePhysicsCreator =
                gameScene.Components.Get<IScenePhysicsCreator>();
            var physicsExecutor =
                gamePhysicsCreator.GetPhysicsExecutor();

            StartPositionSenderTimer();

            if (coroutineRunner == null)
            {
                coroutineRunner = physicsExecutor.GetCoroutineRunner();
            }

            isMoveStopped = false;

            coroutineRunner?.Run(Move());
        }

        public void Stop()
        {
            StopPositionSenderTimer();

            isMoveStopped = true;

            coroutineRunner?.Stop(Move());
        }

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();

            mob = gameObjectGetter.Get();
            proximityChecker = Components.Get<IProximityChecker>();
            mobConfigDataProvider = Components.Get<IMobConfigDataProvider>();
        }

        private IEnumerator Move()
        {
            var startPosition = mob.Transform.Position;
            var position = startPosition;
            var direction = GetRandomDirection();
            var mobConfigData = mobConfigDataProvider.Provide();
            var speed = mobConfigData.Speed;
            var distance = mobConfigData.Distance;

            while (true)
            {
                if (isMoveStopped)
                {
                    yield break;
                }

                if (Vector2.Distance(startPosition, position) >= distance)
                {
                    direction *= -1;
                }

                position += new Vector2(direction, 0) * speed;

                mob.Transform.SetPosition(position);
                yield return null;
            }
        }

        private void SendPosition()
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
    }
}