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

        private ICoroutineRunner coroutineRunner;
        private IProximityChecker proximityChecker;
        private IGameObject mob;

        public MobMoveBehaviour()
        {
            positionSenderTimer = new Timer(100);
            positionSenderTimer.Elapsed += (s, e) => SendPosition();
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            mob = Components.Get<IGameObjectGetter>().Get();
        }

        protected override void OnRemoved()
        {
            StopMove();
        }

        public void SetCoroutineRunner(ICoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }

        public void StartMove()
        {
            positionSenderTimer.Start();
            coroutineRunner.Run(Move());
        }

        public void StopMove()
        {
            positionSenderTimer.Dispose();
            coroutineRunner.Stop(Move());
        }

        private IEnumerator Move()
        {
            var startPosition = mob.Transform.Position;
            var position = startPosition;
            var direction = GetRandomDirection();

            // TODO: Get speed and distance from config
            const float speed = 0.75f;
            const float distance = 2.5f;

            while (true)
            {
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
            foreach (var gameObject in proximityChecker.GetNearbyGameObjects())
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