using CommonTools.Log;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;

namespace Game.Application.SceneObjects
{
    public class BlueSnail : SceneObject
    {
        public BlueSnail(string name, Vector2 position, float direction) 
            : base(name, position, direction)
        {
            var physicsCollisionNotifier = Container.GetComponent<IPhysicsCollisionNotifier>().AssertNotNull();
            physicsCollisionNotifier.CollisionEnter += OnCollisionEnter;

            // TODO: Further implementation needed (Moving a mob, track a player, etc)
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