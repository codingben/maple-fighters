using System;
using System.Collections.Generic;
using Box2DX.Collision;
using Box2DX.Dynamics;
using Common.MathematicsHelper;

namespace Physics.Box2D
{
    public class WorldManager : IWorldManager, IBodyManager, IDisposable
    {
        private readonly World world;

        private readonly LinkedList<NewBodyData> addBodies;
        private readonly LinkedList<BodyData> removeBodies;

        private readonly Dictionary<int, BodyData> bodies;

        public WorldManager(
            Vector2 lowerBound,
            Vector2 upperBound,
            Vector2 gravity,
            bool doSleep = true,
            bool continuousPhysics = false)
        {
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
            world.Dispose();
        }

        public void Update()
        {
            if (removeBodies.Count > 0)
            {
                RemoveBodies();
            }

            if (addBodies.Count > 0)
            {
                AddBodies();
            }
        }

        private void AddBodies()
        {
            foreach (var newBodyData in addBodies)
            {
                var id = newBodyData.Id;
                var body =
                    CreateBody(newBodyData.BodyDef, newBodyData.PolygonDef);
                var bodyData = new BodyData(id, body);

                bodies.Add(newBodyData.Id, bodyData);
            }

            addBodies.Clear();
        }

        private void RemoveBodies()
        {
            foreach (var bodyData in removeBodies)
            {
                world.DestroyBody(bodyData.Body);

                bodies.Remove(bodyData.Id);
            }

            removeBodies.Clear();
        }

        public void AddBody(NewBodyData newBodyData)
        {
            addBodies.AddLast(newBodyData);
        }

        public void RemoveBody(int id)
        {
            var body = bodies[id];

            removeBodies.AddLast(body);
        }

        public void Step(float timeStep, int velocityIterations, int positionIterations)
        {
            world.Step(timeStep, velocityIterations, positionIterations);
        }

        public bool GetBody(int id, out BodyData bodyData)
        {
            return bodies.TryGetValue(id, out bodyData);
        }

        private Body CreateBody(BodyDef bodyDef, PolygonDef polygonDef)
        {
            var body = world.CreateBody(bodyDef);
            body.SetUserData(bodyDef.UserData);
            body.CreateFixture(polygonDef);
            body.SetMassFromShapes();

            return body;
        }
    }
}