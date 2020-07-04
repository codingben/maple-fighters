using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface IWorldManager
    {
        void Update();

        void Step(float timeStep, int velocityIterations, int positionIterations);

        void SetDebugDraw(DebugDraw debugDraw);
    }
}