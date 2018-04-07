using Box2DX.Dynamics;
using CommonTools.Log;
using InterestManagement;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;

namespace Game.Application.GameObjects
{
    public class BodyGameObject : GameObject
    {
        private Body body;
        private readonly BodyDefinitionWrapper bodyDefinitionWrapper;

        protected BodyGameObject(string name, TransformDetails transformDetails, LayerMask layerMask, bool allowSleep = false) 
            : base(name, transformDetails)
        {
            var physicsCollisionNotifier = Components.AddComponent(new PhysicsCollision());
            var fixtureDefinition = PhysicsUtils.CreateFixtureDefinition(transformDetails.Size, layerMask, physicsCollisionNotifier);
            bodyDefinitionWrapper = PhysicsUtils.CreateBodyDefinitionWrapper(fixtureDefinition, transformDetails.Position, this);
            bodyDefinitionWrapper.BodyDefiniton.AllowSleep = allowSleep;
        }

        public override void OnAwake()
        {
            base.OnAwake();

            CreateBody();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            RemoveBody();
        }

        public void CreateBody()
        {
            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var entityManager = presenceSceneProvider.Scene.Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(new BodyInfo(Id, bodyDefinitionWrapper));
        }

        public void RemoveBody()
        {
            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var entityManager = presenceSceneProvider.Scene.Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.RemoveBody(body, Id);
        }

        protected Body GetBody()
        {
            if (body != null)
            {
                return body;
            }

            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var entityManager = presenceSceneProvider.Scene.Components.GetComponent<IEntityManager>().AssertNotNull();
            body = entityManager.GetBody(Id).AssertNotNull();
            return body;
        }
    }
}