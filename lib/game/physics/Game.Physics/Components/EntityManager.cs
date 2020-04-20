using System.Collections.Generic;
using Box2DX.Dynamics;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;

namespace Physics.Box2D.Components
{
    public class EntityManager : IEntityManager
    {
        private readonly World world;
        private readonly List<BodyInfo> addBodies = new List<BodyInfo>(); 
        private readonly List<Body> removeBodies = new List<Body>(); 
        private readonly Dictionary<int, Body> bodies = new Dictionary<int, Body>();

        public EntityManager(World world)
        {
            this.world = world;
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
            foreach (var addBody in addBodies)
            {
                var bodyDefinition = addBody.BodyDefinition;
                var fixtureDefinition = addBody.FixtureDefinition;
                var body = WorldUtils.CreateCharacter(world, bodyDefinition, fixtureDefinition);

                bodies.Add(addBody.Id, body);
            }

            addBodies.Clear();
        }

        private void RemoveBodies()
        {
            foreach (var removeBody in removeBodies)
            {
                world.DestroyBody(removeBody);
            }

            removeBodies.Clear();
        }

        public void AddBody(BodyInfo bodyInfo)
        {
            addBodies.Add(bodyInfo);
        }

        public void RemoveBody(int id)
        {
            var body = bodies[id];
            bodies.Remove(id);

            removeBodies.Add(body);
        }

        public Body GetBody(int id)
        {
            bodies.TryGetValue(id, out var body);

            return body;
        }
    }
}