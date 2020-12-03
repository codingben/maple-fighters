using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.Application.PeerLogic.Components.Interfaces;
using Game.Application.GameObjects.Interfaces;
using MathematicsHelper;
using Game.Common;
using InterestManagement;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;
using Math = System.Math;

namespace Game.Application.GameObjects
{
    public class BlueSnail : MobBase
    {
        private const float MOVE_DIRECTION = 0.01f;
        private const float SLEEP_TIME_AFTER_ATTACK = 0.5f;

        private readonly float speed;
        private readonly float distance;

        private float direction;
        private bool isAttacking;

        private IPositionTransform positionTransform;
        private IDirectionTransform directionTransform;

        private Vector2 lastPosition;

        public BlueSnail(Vector2 position, Vector2 size, float speed, float distance) 
            : base("BlueSnail", position, size)
        {
            this.speed = speed;
            this.distance = distance;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            positionTransform = Components.GetComponent<IPositionTransform>().AssertNotNull();
            directionTransform = Components.GetComponent<IDirectionTransform>().AssertNotNull();

            SubscribeToPositionChanged();
            SubscribeToCollisionEnter();

            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var executor = presenceSceneProvider.GetScene().Components.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPreUpdateExecutor().StartCoroutine(MoveMob());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnubscribeFromPositionChanged();
            UnsubscribeFromCollisionEnter();
        }

        private void SubscribeToCollisionEnter()
        {
            var physicsCollisionNotifier = Components.GetComponent<IPhysicsCollisionNotifier>().AssertNotNull();
            physicsCollisionNotifier.CollisionEnter += OnCollisionEnter;
        }

        private void UnsubscribeFromCollisionEnter()
        {
            var physicsCollisionNotifier = Components.GetComponent<IPhysicsCollisionNotifier>().AssertNotNull();
            physicsCollisionNotifier.CollisionEnter -= OnCollisionEnter;
        }

        private void SubscribeToPositionChanged()
        {
            var positionChangesNotifier = Components.GetComponent<IPositionChangesNotifier>().AssertNotNull();
            positionChangesNotifier.PositionChanged += OnPositionChanged;
        }

        private void UnubscribeFromPositionChanged()
        {
            var positionChangesNotifier = Components.GetComponent<IPositionChangesNotifier>().AssertNotNull();
            positionChangesNotifier.PositionChanged -= OnPositionChanged;
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
                    position += new Vector2(direction, 0) * speed;

                    if (Math.Abs(position.X) > distance)
                    {
                        yield return new WaitForSeconds(0.1f);
                        direction *= -1;
                    }

                    positionTransform.SetPosition(position);
                    directionTransform.SetDirection(direction < 0 ? Direction.Left : Direction.Right);
                    yield return null;
                }

                yield return new WaitForSeconds(SLEEP_TIME_AFTER_ATTACK);

                isAttacking = false;
            }
        }

        private void OnPositionChanged(Vector2 position)
        {
            var body = GetBody().AssertNotNull();
            body.SetXForm(position.FromVector2(), body.GetAngle());

            if (Vector2.Distance(position, lastPosition) > 0.1f)
            {
                UpdatePositionForAll();
                lastPosition = position;
            }

            void UpdatePositionForAll()
            {
                var direction = directionTransform.Direction.GetDirectionsFromDirection();
                var parameters = new SceneObjectPositionChangedEventParameters(Id, position.X, position.Y, direction);
                InterestAreaNotifier.NotifySubscribers((byte)GameEvents.PositionChanged, parameters, MessageSendOptions.DefaultUnreliable((byte)GameDataChannels.Position));
            }
        }

        private void OnCollisionEnter(CollisionInfo collisionInfo, ISceneObject hittedSceneObject)
        {
            var peerIdGetter = hittedSceneObject.Components.GetComponent<IPeerIdGetter>();
            if (peerIdGetter != null && !isAttacking)
            {
                AttackPlayer(peerIdGetter.GetId(), hittedSceneObject, collisionInfo);
            }
        }

        private void AttackPlayer(int peerId, ISceneObject sceneObject, CollisionInfo collisionInfo)
        {
            isAttacking = true;

            var positionTransform = sceneObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
            var orientation = (positionTransform.Position - collisionInfo.Position).Normalize();

            direction = orientation.X < 0 ? -MOVE_DIRECTION : MOVE_DIRECTION;

            RaisePlayerAttacked();

            void RaisePlayerAttacked()
            {
                var parameters = new PlayerAttackedEventParameters(collisionInfo.Position.X, collisionInfo.Position.Y);
                InterestAreaNotifier.NotifySubscriberOnly(peerId, (byte)GameEvents.PlayerAttacked, parameters, MessageSendOptions.DefaultUnreliable());
            }
        }
    }
}