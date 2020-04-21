using System;
using System.Collections.Generic;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D;

namespace Physics.Box2D.Components
{
    public class EntityManager : IDisposable
    {
        private readonly IWorldProvider worldProvider;

        private readonly LinkedList<BodyData> addBodies;
        private readonly LinkedList<BodyData> removeBodies;

        private readonly Dictionary<int, BodyData> bodies;

        public EntityManager(IWorldProvider worldProvider)
        {
            this.worldProvider = worldProvider;

            addBodies = new LinkedList<BodyData>();
            removeBodies = new LinkedList<BodyData>();

            bodies = new Dictionary<int, BodyData>();
        }

        public void Dispose()
        {
            RemoveAllBodies();
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
            foreach (var bodyData in addBodies)
            {
                var world = worldProvider.Provide();
                if (world != null)
                {
                    bodies.Add(bodyData.Id, bodyData);
                }
            }

            addBodies.Clear();
        }

        private void RemoveBodies()
        {
            foreach (var bodyData in removeBodies)
            {
                var world = worldProvider.Provide();
                world?.DestroyBody(bodyData.Body);
            }

            removeBodies.Clear();
        }

        public void AddBody(BodyData bodyData)
        {
            addBodies.AddLast(bodyData);
        }

        public void RemoveBody(int id)
        {
            var body = bodies[id];
            bodies.Remove(id);

            removeBodies.AddLast(body);
        }

        public void RemoveAllBodies()
        {
            foreach (var body in bodies.Values)
            {
                removeBodies.AddLast(body);
            }

            bodies.Clear();
        }

        public BodyData GetBody(int id)
        {
            bodies.TryGetValue(id, out var body);

            return body;
        }
    }
}