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
        private const float DIRECTION_TIME = 1000;

        private readonly Timer directionTimer;
        private readonly Random random = new();

        private IGameObject mob;
        private IProximityChecker proximityChecker;
        private IMobConfigDataProvider mobConfigDataProvider;
        private ICoroutineRunner coroutineRunner;

        private float direction;
        private bool isMoveStopped;

        public MobMoveBehaviour()
        {
            directionTimer = new Timer(DIRECTION_TIME);
            directionTimer.Elapsed += (_, _) => ChangeDirection();
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            mobConfigDataProvider = Components.Get<IMobConfigDataProvider>();
            mob = Components.Get<IGameObjectGetter>().Get();
            mob.Transform.PositionChanged += OnPositionChanged;
        }

        protected override void OnRemoved()
        {
            Stop();

            directionTimer?.Dispose();
        }

        public void Start()
        {
            if (coroutineRunner == null)
            {
                coroutineRunner = GetCoroutineRunner();
            }

            isMoveStopped = false;

            directionTimer?.Start();
            coroutineRunner?.Run(Move());
        }

        public void Stop()
        {
            isMoveStopped = true;

            directionTimer?.Stop();
            coroutineRunner?.Stop(Move());
        }

        private IEnumerator Move()
        {
            var position = mob.Transform.Position;
            var mobConfigData = mobConfigDataProvider.Provide();
            var speed = mobConfigData.Speed;

            direction = GetRandomDirection();

            while (!isMoveStopped)
            {
                position += new Vector2(direction, 0) * speed;
                mob.Transform.SetPosition(position);
                yield return null;
            }
        }

        private void ChangeDirection()
        {
            direction *= -1;
        }

        private void OnPositionChanged()
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