using System.Collections.Generic;
using Box2DX.Dynamics;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;

namespace Physics.Box2D.Components
{
    public class EntityManager : IEntityManager
    {
        private readonly IWorldProvider worldProvider;
        private readonly IList<BodyInfo> addBodies = new List<BodyInfo>(); 
        private readonly IList<Body> removeBodies = new List<Body>(); 
        private readonly IDictionary<int, Body> bodies = new Dictionary<int, Body>();

        public EntityManager(IWorldProvider worldProvider)
        {
            this.worldProvider = worldProvider;
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
                var world = worldProvider.GetWorld();
                if (world != null)
                {
                    var newBody = WorldUtils.CreateCharacter(
                        world,
                        addBody.BodyDefinition,
                        addBody.FixtureDefinition);

                    bodies.Add(addBody.Id, newBody);
                }
            }

            addBodies.Clear();
        }

        private void RemoveBodies()
        {
            foreach (var removeBody in removeBodies)
            {
                var world = worldProvider.GetWorld();
                world?.DestroyBody(removeBody);
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