using System;
using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface IPhysicsWorldManager : IDisposable
    {
        void AddBody(NewBodyData newBodyData);

        void RemoveBody(int id);

        bool GetBody(int id, out BodyData bodyData);

        void UpdateBodies();

        void Step(float timeStep, int velocityIterations, int positionIterations);

        void SetDebugDraw(DebugDraw debugDraw);
    }
}