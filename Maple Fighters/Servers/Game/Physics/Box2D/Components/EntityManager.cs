using System.Collections.Generic;
using Box2DX.Dynamics;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;

namespace Physics.Box2D
{
    public class EntityManager : Component, IEntityManager
    {
        private World world;

        private readonly List<BodyInfo> addBodies = new List<BodyInfo>(); 
        private readonly List<Body> removeBodies = new List<Body>(); 

        private readonly Dictionary<int, Body> bodies = new Dictionary<int, Body>();

        protected override void OnAwake()
        {
            base.OnAwake();

            var physicsWorldProvider = Components.GetComponent<IPhysicsWorldProvider>().AssertNotNull();
            world = physicsWorldProvider.GetWorld();

            var executor = Components.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPostUpdateExecutor().StartCoroutine(Update());
        }

        private IEnumerator<IYieldInstruction> Update()
        {
            while (true)
            {
                if (removeBodies.Count > 0)
                {
                    RemoveBodies();
                }

                if (addBodies.Count > 0)
                {
                    AddBodies();
                }
                yield return null;
            }
        }

        private void AddBodies()
        {
            foreach (var addBody in addBodies)
            {
                var body = world.CreateCharacter(addBody.BodyDefinition, addBody.BodyDefinition.FixtureDefinition);
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

        public void RemoveBody(Body body, int id)
        {
            bodies.Remove(id);
            removeBodies.Add(body);
        }

        public Body GetBody(int id)
        {
            return bodies.TryGetValue(id, out var body) ? body : null;
        }
    }
}