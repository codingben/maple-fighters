using System;
using ComponentModel.Common;
using Game.InterestManagement;
using Physics.Box2D;

namespace Game.Application.SceneObjects
{
    public class PhysicsCollisionNotifier : Component<ISceneObject>, IPhysicsCollisionNotifier, IPhysicsCollision
    {
        public event Action<CollisionInfo> CollisionEnter; 
        public event Action<CollisionInfo> CollisionExit;

        protected override void OnDestroy()
        {
            base.OnDestroy();

            CollisionEnter = null;
            CollisionExit = null;
        }

        public void OnCollisionEnter(CollisionInfo collisionInfo)
        {
            CollisionEnter?.Invoke(collisionInfo);
        }

        public void OnCollisionExit(CollisionInfo collisionInfo)
        {
            CollisionExit?.Invoke(collisionInfo);
        }
    }
}