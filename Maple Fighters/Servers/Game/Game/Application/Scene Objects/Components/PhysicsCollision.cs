using System;
using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Interfaces;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Core;

namespace Game.Application.SceneObjects
{
    public class PhysicsCollision : Component<ISceneObject>, IPhysicsCollisionNotifier, IPhysicsCollisionCallback
    {
        public event Action<CollisionInfo, ISceneObject> CollisionEnter; 
        public event Action<CollisionInfo, ISceneObject> CollisionExit;

        // NOTE: It may be called twice since two colliders will do an interaction. That would prevent it.
        private readonly Dictionary<int, bool> hittedSceneObjects = new Dictionary<int, bool>();

        public void OnCollisionEnter(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Body.GetUserData() is ISceneObject hittedSceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace("Could not get data from a body."));
                return;
            }

            if (!hittedSceneObjects.ContainsKey(hittedSceneObject.Id))
            {
                hittedSceneObjects.Add(hittedSceneObject.Id, true);
                CollisionEnter?.Invoke(collisionInfo, hittedSceneObject);
            }
        }

        public void OnCollisionExit(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Body.GetUserData() is ISceneObject hittedSceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace("Could not get data from a body."));
                return;
            }

            if (hittedSceneObjects.ContainsKey(hittedSceneObject.Id))
            {
                hittedSceneObjects.Remove(hittedSceneObject.Id);
                CollisionExit?.Invoke(collisionInfo, hittedSceneObject);
            }
        }
    }
}