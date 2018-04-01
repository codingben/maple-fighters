using System;
using Physics.Box2D;

namespace Game.Application.SceneObjects
{
    public interface IPhysicsCollisionNotifier
    {
        event Action<CollisionInfo> CollisionEnter;
        event Action<CollisionInfo> CollisionExit;
    }
}