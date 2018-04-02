using System;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Core;

namespace Game.Application.SceneObjects.Interfaces
{
    public interface IPhysicsCollisionNotifier
    {
        event Action<CollisionInfo, ISceneObject> CollisionEnter;
        event Action<CollisionInfo, ISceneObject> CollisionExit;
    }
}