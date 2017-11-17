using System.Collections.Generic;
using Box2DX.Dynamics;
using CommonTools.Coroutines;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;

namespace Physics.Box2D
{
    public class EntityManager : Component<ISceneEntity>, IEntityManager
    {
        private World world;

        private readonly List<BodyInfo> addBodies = new List<BodyInfo>(); 
        private readonly List<Body> removeBodies = new List<Body>(); 

        private readonly Dictionary<int, Body> bodies = new Dictionary<int, Body>();

        protected override void OnAwake()
        {
            base.OnAwake();

            var physicsWorldProvider = Entity.Container.GetComponent<IPhysicsWorldProvider>().AssertNotNull();
            world = physicsWorldProvider.GetWorld();

            var executor = Entity.Container.GetComponent<ISceneOrderExecutor>().AssertNotNull();
            executor.GetPostUpdateExecutor().StartCoroutine(Update());
        }

        private IEnumerator<IYieldInstruction> Update()
        {
            while (true)
            {
                foreach (var addBody in addBodies)
                {
                    var body = world.CreateCharacter(addBody.Body, addBody.Body.FixtureDefinition);
                    bodies.Add(addBody.Id, body);
                }

                foreach (var removeBody in removeBodies)
                {
                    world.DestroyBody(removeBody);
                }

                addBodies.Clear();
                removeBodies.Clear();
                yield return null;
            }
        }

        public void AddBody(BodyInfo bodyInfo)
        {
            addBodies.Add(bodyInfo);
        }

        public void RemoveBody(int id)
        {
            var body = GetBody(id);
            if (body == null)
            {
                return;
            }

            bodies.Remove(id);
            removeBodies.Add(body);
        }

        public Body GetBody(int id)
        {
            if (bodies.TryGetValue(id, out var body))
            {
                return body;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find body id #{id}"));
            return null;
        }
    }
}