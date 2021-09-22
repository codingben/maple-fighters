using System.Collections;
using System.Timers;
using Common.ComponentModel;
using Common.MathematicsHelper;
using Coroutines;
using Game.Messages;

namespace Game.Application.Objects.Components
{
    public class BlueSnailMoveBehaviour : ComponentBase
    {
        private readonly ICoroutineRunner coroutineRunner;
        private readonly Timer positionSenderTimer;
        private readonly Timer directionSetterTimer;

        private IProximityChecker proximityChecker;
        private IGameObject blueSnail;

        private float direction = 0.1f;

        public BlueSnailMoveBehaviour(ICoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;

            positionSenderTimer = new Timer(100);
            directionSetterTimer = new Timer(3500);

            directionSetterTimer.Elapsed += (s, e) => ChangeDirection();
            positionSenderTimer.Elapsed += (s, e) => SendPosition();
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            blueSnail = Components.Get<IGameObjectGetter>().Get();

            positionSenderTimer.Start();
            directionSetterTimer.Start();

            coroutineRunner.Run(Move());
        }

        protected override void OnRemoved()
        {
            positionSenderTimer.Dispose();
            directionSetterTimer.Dispose();

            coroutineRunner.Stop(Move());
        }

        private IEnumerator Move()
        {
            var position = blueSnail.Transform.Position;
            var speed = 0.75f;

            while (true)
            {
                position += new Vector2(direction, 0) * speed;
                blueSnail.Transform.SetPosition(position);
                yield return null;
            }
        }

        private void SendPosition()
        {
            foreach (var gameObject in proximityChecker.GetNearbyGameObjects())
            {
                var message = new PositionChangedMessage()
                {
                    GameObjectId = blueSnail.Id,
                    X = blueSnail.Transform.Position.X,
                    Y = blueSnail.Transform.Position.Y
                };
                var messageSender = gameObject.Components.Get<IMessageSender>();
                messageSender?.SendMessage((byte)MessageCodes.PositionChanged, message);
            }
        }

        private void ChangeDirection()
        {
            direction *= -1;
        }
    }
}