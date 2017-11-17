using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
using Shared.Game.Common;
using Math = System.Math;

namespace Game.Application.SceneObjects
{
    public class BlueSnail : Mob
    {
        private readonly Vector2 bodySize;
        private readonly float moveSpeed;
        private readonly float moveDistance;

        public BlueSnail(Vector2 position, Vector2 bodySize, float moveSpeed, float moveDistance) 
            : base("BlueSnail", position)
        {
            this.bodySize = bodySize;
            this.moveSpeed = moveSpeed;
            this.moveDistance = moveDistance;
        }

        public override void OnAwake()
        {
            base.OnAwake();

            var physicsCollisionProvider = Container.AddComponent(new PhysicsCollisionNotifier());
            var fixtureDefinition = PhysicsUtils.CreateFixtureDefinition(bodySize, LayerMask.Mob, physicsCollisionProvider);

            var transform = Container.GetComponent<ITransform>().AssertNotNull();
            var bodyDefinitionWrapper = PhysicsUtils.CreateBodyDefinitionWrapper(fixtureDefinition, transform.InitialPosition, this);

            var entityManager = Scene.Container.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(new BodyInfo(Id, bodyDefinitionWrapper));

            var physicsCollisionNotifier = Container.GetComponent<IPhysicsCollisionNotifier>().AssertNotNull();
            physicsCollisionNotifier.CollisionEnter += OnCollisionEnter;

            var executor = Scene.Container.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPreUpdateExecutor().StartCoroutine(MoveMob());
        }

        private IEnumerator<IYieldInstruction> MoveMob()
        {
            yield return new WaitForSeconds(5);

            var entityManager = Scene.Container.GetComponent<IEntityManager>().AssertNotNull();
            var body = entityManager.GetBody(Id).AssertNotNull();

            var transform = Container.GetComponent<ITransform>().AssertNotNull();
            var position = body.GetPosition().ToVector2();
            var direction = 0.1f;

            while (true)
            {
                position += new Vector2(direction, 0);

                if(Math.Abs(position.X) > moveDistance)
                {
                    direction *= -1;
                }

                body.MoveBody(position, moveSpeed, false);
                transform.SetPosition(position, direction > 0 ? Directions.Right : Directions.Left);
                yield return null;
            }
        }

        private void OnPositionChanged(Vector2 position, Directions direction)
        {
            var parameters = new SceneObjectPositionChangedEventParameters(Id, position.X, position.Y, direction);
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position);
            InterestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, messageSendOptions);
        }

        private void OnCollisionEnter(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Body.GetUserData() is ISceneObject hittedSceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace("Could not get data from a body."));
                return;
            }

            LogUtils.Log(MessageBuilder.Trace($"Hitting a player with id: {hittedSceneObject.Id}"));

            // TODO: NOTE: It may be called twice since two colliders will do an interaction.

            // TODO: Implement - Send to the player new properites which will include his HP.
            // TODO: Implement - Send impulse to the player's body from to his direction. 
        }
    }
}