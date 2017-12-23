using System;
using ComponentModel.Common;
using Physics.Box2D;

namespace Game.Application.SceneObjects
{
    public interface IPhysicsCollisionNotifier : IExposableComponent
    {
        event Action<CollisionInfo> CollisionEnter;
        event Action<CollisionInfo> CollisionExit;
    }
}