using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Dynamics;
using InterestManagement;

namespace Game.Physics
{
    public class PhysicsWorldManager : IPhysicsWorldManager
    {
        private readonly Queue<NewBodyData> addBodies;
        private readonly Queue<BodyData> removeBodies;
        private readonly Dictionary<int, BodyData> bodies;
        private readonly World world;

        public PhysicsWorldManager(
            Vector2 lowerBound,
            Vector2 upperBound,
            Vector2 gravity,
            bool doSleep = false,
            bool continuousPhysics = false)
        {
            addBodies = new Queue<NewBodyData>();
            removeBodies = new Queue<BodyData>();
            bodies = new Dictionary<int, BodyData>();

            var worldAabb = new AABB
            {
                LowerBound = lowerBound.FromVector2(),
                UpperBound = upperBound.FromVector2()
            };

            world = new World(worldAabb, gravity.FromVector2(), doSleep);
            world.SetContactFilter(new GroupContactFilter());
            world.SetContactListener(new BodyContactListener());
            world.SetContinuousPhysics(continuousPhysics);
        }

        public void Dispose()
        {
            world?.Dispose();
        }

        public void UpdateBodies()
        {
            RemoveBodies();
            AddBodies();
        }

        private void AddBodies()
        {
            if (addBodies.Count == 0) return;

            while (addBodies.TryDequeue(out var newBodyData))
            {
                var id = newBodyData.Id;

                if (bodies.ContainsKey(id))
                {
                    continue;
                }

                var body =
                    CreateBody(newBodyData.BodyDef, newBodyData.PolygonDef);
                var bodyData = new BodyData(id, body);

                bodies.Add(id, bodyData);
            }
        }

        private void RemoveBodies()
        {
            if (removeBodies.Count == 0) return;

            while (removeBodies.TryDequeue(out var bodyData))
            {
                world?.DestroyBody(bodyData.Body);

                var id = bodyData.Id;

                if (bodies.ContainsKey(id))
                {
                    bodies.Remove(id);
                }
            }
        }

        public void AddBody(NewBodyData newBodyData)
        {
            addBodies.Enqueue(newBodyData);
        }

        public void RemoveBody(int id)
        {
            var body = bodies[id];

            removeBodies.Enqueue(body);
        }

        public void Step(float timeStep, int velocityIterations, int positionIterations)
        {
            world?.Step(timeStep, velocityIterations, positionIterations);
        }

        public void SetDebugDraw(DebugDraw debugDraw)
        {
            world?.SetDebugDraw(debugDraw);
        }

        public bool GetBody(int id, out BodyData bodyData)
        {
            return bodies.TryGetValue(id, out bodyData);
        }

        private Body CreateBody(BodyDef bodyDef, PolygonDef polygonDef)
        {
            var body = world?.CreateBody(bodyDef);

            body?.SetUserData(bodyDef.UserData);
            body?.CreateFixture(polygonDef);
            body?.SetMassFromShapes();

            return body;
        }
    }
}