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
        private readonly float moveSpeed;
        private readonly float moveDistance;

        public BlueSnail(Vector2 position, Vector2 bodySize, float moveSpeed, float moveDistance) 
            : base("BlueSnail", position, bodySize)
        {
            this.moveSpeed = moveSpeed;
            this.moveDistance = moveDistance;
        }

        public override void OnAwake()
        {
            base.OnAwake();

            CreateBody();

            var physicsCollisionNotifier = Container.GetComponent<IPhysicsCollisionNotifier>();
            physicsCollisionNotifier.CollisionEnter += OnCollisionEnter;

            var executor = Scene.Container.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPreUpdateExecutor().StartCoroutine(MoveMob());
        }

        private IEnumerator<IYieldInstruction> MoveMob()
        {
            var transform = Container.GetComponent<ITransform>().AssertNotNull();
            transform.PositionDirectionChanged += OnPositionChanged;

            yield return new WaitForSeconds(1);

            var position = GetBody().GetPosition().ToVector2();
            var direction = 0.01f;

            while (true)
            {
                position += new Vector2(direction, 0);

                if(Math.Abs(position.X) > moveDistance)
                {
                    yield return new WaitForSeconds(0.1f);
                    direction *= -1;
                }

                transform.SetPosition(position, direction > 0 ? Directions.Right : Directions.Left);
                yield return null;
            }
        }

        private void OnPositionChanged(Vector2 position, Directions direction)
        {
            GetBody().MoveBody(position, moveSpeed, false);

            var parameters = new SceneObjectPositionChangedEventParameters(Id, position.X, position.Y, direction);
            InterestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
        }

        private void OnCollisionEnter(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Body.GetUserData() is ISceneObject hittedSceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace("Could not get data from a body."));
                return;
            }

            LogUtils.Log(MessageBuilder.Trace($"Hitting a player with id: {hittedSceneObject.Id}"));

            // TODO: Implement - Send to the player new properites which will include his HP.

            AttackPlayer(hittedSceneObject, collisionInfo);
        }

        private void AttackPlayer(ISceneObject sceneObject, CollisionInfo collisionInfo)
        {
            var parameters = new PlayerAttackedEventParameters(collisionInfo.Position.X, collisionInfo.Position.Y);
            InterestAreaNotifier.NotifySubscriberOnly(sceneObject, (byte)GameEvents.PlayerAttacked, parameters, MessageSendOptions.DefaultUnreliable());
        }
    }
}