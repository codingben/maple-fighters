using System;
using Box2DX.Dynamics;

namespace Physics.Box2D
{
    public interface IWorldWrapper : IDisposable
    {
        Body CreateBody(BodyDef bodyDefinition, PolygonDef polygonDefinition);

        void DestroyBody(Body body);

        World GetWorld();
    }
}