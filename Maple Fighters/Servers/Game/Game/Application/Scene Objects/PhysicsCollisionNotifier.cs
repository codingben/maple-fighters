using System;
using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using Physics.Box2D;

namespace Game.Application.SceneObjects
{
    public class PhysicsCollisionNotifier : Component<ISceneObject>, IPhysicsCollisionNotifier, IPhysicsCollision
    {
        public event Action<CollisionInfo> CollisionEnter; 
        public event Action<CollisionInfo> CollisionExit;

        // NOTE: It may be called twice since two colliders will do an interaction. That would prevent it.
        private readonly Dictionary<int, bool> hittedSceneObjects = new Dictionary<int, bool>();

        protected override void OnDestroy()
        {
            base.OnDestroy();

            CollisionEnter = null;
            CollisionExit = null;
        }

        public void OnCollisionEnter(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Body.GetUserData() is ISceneObject hittedSceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace("Could not get data from a body."));
                return;
            }

            if (hittedSceneObjects.ContainsKey(hittedSceneObject.Id))
            {
                return;
            }

            hittedSceneObjects.Add(hittedSceneObject.Id, true);

            CollisionEnter?.Invoke(collisionInfo);
        }

        public void OnCollisionExit(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Body.GetUserData() is ISceneObject hittedSceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace("Could not get data from a body."));
                return;
            }

            if (!hittedSceneObjects.ContainsKey(hittedSceneObject.Id))
            {
                return;
            }

            hittedSceneObjects.Remove(hittedSceneObject.Id);

            CollisionExit?.Invoke(collisionInfo);
        }
    }
}