using System;
using System.Collections.Generic;

namespace Physics.Box2D
{
    public class EntityManager : IDisposable
    {
        private readonly IWorldWrapper worldWrapper;

        private readonly LinkedList<BodyData> addBodies;
        private readonly LinkedList<BodyData> removeBodies;
        private readonly Dictionary<int, BodyData> bodies;

        public EntityManager(IWorldWrapper worldWrapper)
        {
            this.worldWrapper = worldWrapper;

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
                bodies.Add(bodyData.Id, bodyData);
            }

            addBodies.Clear();
        }

        private void RemoveBodies()
        {
            foreach (var bodyData in removeBodies)
            {
                worldWrapper?.DestroyBody(bodyData.Body);
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