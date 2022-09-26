using System;
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
            try
            {
                world?.Dispose();
            }
            catch (Exception)
            {
                // Left blank intentionally
            }
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
                if (body == null)
                {
                    continue;
                }

                var bodyData = new BodyData(id, body);
                bodies.Add(id, bodyData);
            }
        }

        private void RemoveBodies()
        {
            if (removeBodies.Count == 0) return;

            while (removeBodies.TryDequeue(out var bodyData))
            {
                var id = bodyData.Id;

                if (bodies.ContainsKey(id))
                {
                    bodies.Remove(id);
                }

                try
                {
                    world?.DestroyBody(bodyData.Body);
                }
                catch (Exception)
                {
                    // Left blank intentionally
                }
            }
        }

        public void AddBody(NewBodyData newBodyData)
        {
            addBodies.Enqueue(newBodyData);
        }

        public void RemoveBody(int id)
        {
            if (GetBody(id, out var body))
            {
                removeBodies.Enqueue(body);
            }
        }

        public void Step(float timeStep, int velocityIterations, int positionIterations)
        {
            try
            {
                world?.Step(timeStep, velocityIterations, positionIterations);
            }
            catch (Exception)
            {
                // Left blank intentionally
            }
        }

        public void SetDebugDraw(DebugDraw debugDraw)
        {
            try
            {
                world?.SetDebugDraw(debugDraw);
            }
            catch (Exception)
            {
                // Left blank intentionally
            }
        }

        public bool GetBody(int id, out BodyData bodyData)
        {
            return bodies.TryGetValue(id, out bodyData);
        }

        private Body CreateBody(BodyDef bodyDef, PolygonDef polygonDef)
        {
            try
            {
                var body = world?.CreateBody(bodyDef);

                body?.SetUserData(bodyDef.UserData);
                body?.CreateFixture(polygonDef);
                body?.SetMassFromShapes();

                return body;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}