using System;
using System.Collections;
using System.Timers;
using Coroutines;
using Game.Application.Components;
using Game.Messages;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    public class MobMoveBehaviour : ComponentBase
    {
        private readonly Timer positionSenderTimer;
        private readonly Random random = new();

        private IGameObject gameObject;
        private IProximityChecker proximityChecker;
        private ICoroutineRunner coroutineRunner;
        private IMobConfigDataProvider mobConfigDataProvider;

        public MobMoveBehaviour()
        {
            positionSenderTimer = new Timer(100);
            positionSenderTimer.Elapsed += (s, e) => SendPosition();
            positionSenderTimer.Start();
        }

        protected override void OnAwake()
        {
            gameObject = Components.Get<IGameObjectGetter>().Get();
            proximityChecker = Components.Get<IProximityChecker>();
            mobConfigDataProvider = Components.Get<IMobConfigDataProvider>();

            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            presenceSceneProvider.SceneChanged += (gameScene) =>
            {
                var gamePhysicsCreator =
                    gameScene.Components.Get<IScenePhysicsCreator>();
                var physicsExecutor =
                    gamePhysicsCreator.GetPhysicsExecutor();

                coroutineRunner = physicsExecutor.GetCoroutineRunner();
                coroutineRunner.Run(Move());
            };
        }

        private IEnumerator Move()
        {
            var startPosition = gameObject.Transform.Position;
            var position = startPosition;
            var direction = GetRandomDirection();
            var mobConfigData = mobConfigDataProvider.Provide();
            var speed = mobConfigData.Speed;
            var distance = mobConfigData.Distance;

            while (true)
            {
                if (Vector2.Distance(startPosition, position) >= distance)
                {
                    direction *= -1;
                }

                position += new Vector2(direction, 0) * speed;

                gameObject.Transform.SetPosition(position);
                yield return null;
            }
        }

        private void SendPosition()
        {
            var nearbyGameObjects = proximityChecker?.GetNearbyGameObjects();

            foreach (var nearbyGameObject in nearbyGameObjects)
            {
                var message = new PositionChangedMessage()
                {
                    GameObjectId = gameObject.Id,
                    X = gameObject.Transform.Position.X,
                    Y = gameObject.Transform.Position.Y
                };
                var messageSender = nearbyGameObject.Components.Get<IMessageSender>();

                messageSender?.SendMessage((byte)MessageCodes.PositionChanged, message);
            }
        }

        private float GetRandomDirection()
        {
            return random.Next(-1, 1) == 0 ? 0.1f : -0.1f;
        }
    }
}