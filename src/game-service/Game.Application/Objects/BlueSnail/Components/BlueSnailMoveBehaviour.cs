using System;
using System.Collections;
using Common.ComponentModel;
using Common.MathematicsHelper;
using Coroutines;
using Game.Physics;
using Game.Messages;

namespace Game.Application.Objects.Components
{
    public class BlueSnailMoveBehaviour : ComponentBase
    {
        private readonly ICoroutineRunner coroutineRunner;
        private readonly IPhysicsWorldManager physicsWorldManager;

        private IProximityChecker proximityChecker;
        private IGameObject blueSnail;

        private BodyData? bodyData;

        public BlueSnailMoveBehaviour(ICoroutineRunner coroutineRunner, IPhysicsWorldManager physicsWorldManager)
        {
            this.coroutineRunner = coroutineRunner;
            this.physicsWorldManager = physicsWorldManager;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
            blueSnail = Components.Get<IGameObjectGetter>().Get();
            blueSnail.Transform.PositionChanged += OnPositionChanged;

            coroutineRunner.Run(Move());
        }

        protected override void OnRemoved()
        {
            blueSnail.Transform.PositionChanged -= OnPositionChanged;

            coroutineRunner.Stop(Move());
        }

        private IEnumerator Move()
        {
            var id = blueSnail.Id;
            var position = blueSnail.Transform.Position;
            var direction = 0.1f;
            var speed = 0.5f;
            var distance = 2f;

            if (physicsWorldManager.GetBody(id, out var value))
            {
                bodyData = value;
            }

            while (true)
            {
                position += new Vector2(direction, 0) * speed;

                if (Math.Abs(position.X) > distance)
                {
                    direction *= -1;
                }

                var body = bodyData?.Body;
                if (body != null)
                {
                    body.SetXForm(position.FromVector2(), body.GetAngle());
                }

                blueSnail.Transform.SetPosition(position);
                yield return null;
            }
        }

        private void OnPositionChanged()
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
    }
}