using Box2DX.Dynamics;
using CommonTools.Log;
using Game.Application.GameObjects.Interfaces;
using InterestManagement;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;

namespace Game.Application.GameObjects
{
    public class BodyGameObject : GameObject
    {
        private Body body;
        private readonly LayerMask layerMask;
        private readonly bool allowSleep;

        protected BodyGameObject(string name, TransformDetails transformDetails, LayerMask layerMask, bool allowSleep = false) 
            : base(name, transformDetails)
        {
            this.layerMask = layerMask;
            this.allowSleep = allowSleep;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            Components.AddComponent(new PhysicsCollision());

            CreateBody();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            RemoveBody();
        }

        public void CreateBody()
        {
            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var entityManager = presenceSceneProvider.GetScene().Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(GetBodyInfo());
        }

        public void RemoveBody()
        {
            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var entityManager = presenceSceneProvider.GetScene().Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.RemoveBody(Id);
        }

        protected Body GetBody()
        {
            if (body != null)
            {
                return body;
            }

            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var entityManager = presenceSceneProvider.GetScene().Components.GetComponent<IEntityManager>().AssertNotNull();
            body = entityManager.GetBody(Id).AssertNotNull();
            return body;
        }

        private BodyInfo GetBodyInfo()
        {
            var sizeTransform = Components.GetComponent<ISizeTransform>().AssertNotNull();
            var positionTransform = Components.GetComponent<IPositionTransform>().AssertNotNull();
            var physicsCollisionNotifier = Components.GetComponent<IPhysicsCollisionNotifier>().AssertNotNull();
            var fixtureDefinition = PhysicsUtils.CreateFixtureDefinition(sizeTransform.Size, layerMask, physicsCollisionNotifier);
            var bodyDefinitionWrapper = PhysicsUtils.CreateBodyDefinitionWrapper(fixtureDefinition, positionTransform.Position, this);
            bodyDefinitionWrapper.BodyDefiniton.AllowSleep = allowSleep;
            return new BodyInfo(Id, bodyDefinitionWrapper);
        }
    }
}