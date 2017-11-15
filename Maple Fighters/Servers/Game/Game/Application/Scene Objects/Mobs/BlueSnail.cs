using System.Collections.Generic;
using CommonTools.Coroutines;
using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components.Coroutines;
using Shared.Game.Common;
using Math = System.Math;

namespace Game.Application.SceneObjects
{
    public class BlueSnail : Mob
    {
        private const string MOB_NAME = "BlueSnail";
        private ICoroutinesExecuter coroutinesExecutor;

        public BlueSnail(Vector2 position, Vector2 size, float direction) 
            : base(MOB_NAME, position, size, direction)
        {
            // TODO: Further implementation needed (Track a player, etc)
        }

        public override void OnAwake()
        {
            base.OnAwake();

            CreateCharacter();

            var physicsCollisionNotifier = Container.GetComponent<IPhysicsCollisionNotifier>().AssertNotNull();
            physicsCollisionNotifier.CollisionEnter += OnCollisionEnter;

            coroutinesExecutor = Server.Entity.Container.GetComponent<ICoroutinesExecuter>().AssertNotNull();
            coroutinesExecutor.StartCoroutine(MoveMob());
        }

        private IEnumerator<IYieldInstruction> MoveMob()
        {
            yield return new WaitForSeconds(5);

            var transform = Container.GetComponent<ITransform>().AssertNotNull();
            var position = Body.GetPosition().ToVector2();
            var direction = 0.1f;

            while (true)
            {
                position += new Vector2(direction, 0);

                if(Math.Abs(position.X) > 3.5f)
                {
                    direction *= -1;
                }

                transform.SetPosition(position, direction > 0 ? Directions.Right : Directions.Left);
                yield return null;
            }
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