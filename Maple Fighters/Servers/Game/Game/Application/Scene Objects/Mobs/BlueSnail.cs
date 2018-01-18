using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
using Shared.Game.Common;
using Math = System.Math;

namespace Game.Application.SceneObjects
{
    public class BlueSnail : Mob
    {
        private const float MOVE_DIRECTION = 0.01f;
        private const float SLEEP_TIME_AFTER_ATTACK = 0.5f;

        private readonly float speed; // TODO: Implement
        private readonly float distance;

        private float direction;
        private bool isAttacking;

        private ITransform transform;
        private IOrientationProvider orientationProvider;

        public BlueSnail(Vector2 position, Vector2 bodySize, float speed, float distance) 
            : base("BlueSnail", position, bodySize)
        {
            this.speed = speed;
            this.distance = distance;
        }

        public override void OnAwake()
        {
            base.OnAwake();

            CreateBody();

            orientationProvider = Container.GetComponent<IOrientationProvider>().AssertNotNull();
            transform = Container.GetComponent<ITransform>().AssertNotNull();
            transform.PositionChanged += OnPositionChanged;

            var physicsCollisionNotifier = Container.GetComponent<IPhysicsCollisionNotifier>().AssertNotNull();
            physicsCollisionNotifier.CollisionEnter += OnCollisionEnter;

            var presenceSceneProvider = Container.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var executor = presenceSceneProvider.Scene.Entity.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPreUpdateExecutor().StartCoroutine(MoveMob());
        }

        private IEnumerator<IYieldInstruction> MoveMob()
        {
            yield return new WaitForSeconds(0.1f);

            var position = GetBody().GetPosition().ToVector2();
            direction = MOVE_DIRECTION;

            while (true)
            {
                while (!isAttacking)
                {
                    position += new Vector2(direction, 0);

                    if (Math.Abs(position.X) > distance)
                    {
                        yield return new WaitForSeconds(0.1f);
                        direction *= -1;
                    }

                    orientationProvider.Direction = direction < 0 ? Direction.Left : Direction.Right;
                    transform.SetPosition(position);
                    yield return null;
                }

                yield return new WaitForSeconds(SLEEP_TIME_AFTER_ATTACK);

                isAttacking = false;
            }
        }

        private void OnPositionChanged(Vector2 position)
        {
            /* 
             * NOTE: Deprecated MoveBody() due to forces and velocity issues between two fixtures.
               -> GetBody().MoveBody(position, moveSpeed, true); 
            */

            GetBody().SetXForm(position.FromVector2(), GetBody().GetAngle());

            var direction = orientationProvider.Direction.GetDirectionsFromDirection();

            var parameters = new SceneObjectPositionChangedEventParameters(Id, position.X, position.Y, direction);
            var sendOptions = MessageSendOptions.DefaultUnreliable((byte) GameDataChannels.Position);
            InterestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, sendOptions);
        }

        private void OnCollisionEnter(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Body.GetUserData() is ISceneObject hittedSceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace("Could not get data from a body."));
                return;
            }

            // TODO: Implement - Send to the player new properites which will include his HP.

            if (!isAttacking)
            {
                AttackPlayer(hittedSceneObject, collisionInfo);
            }
        }

        private void AttackPlayer(ISceneObject sceneObject, CollisionInfo collisionInfo)
        {
            isAttacking = true;

            var transform = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            var orientation = (transform.Position - collisionInfo.Position).Normalize();
            direction = orientation.X < 0 ? -MOVE_DIRECTION : MOVE_DIRECTION;

            var parameters = new PlayerAttackedEventParameters(collisionInfo.Position.X, collisionInfo.Position.Y);
            InterestAreaNotifier.NotifySubscriberOnly(sceneObject, (byte)GameEvents.PlayerAttacked, parameters, MessageSendOptions.DefaultUnreliable());
        }
    }
}