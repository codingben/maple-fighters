using Box2DX.Dynamics;
using CommonTools.Log;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
using ServerApplication.Common.Components;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.SceneObjects
{
    /// <summary>
    /// Will create a body (and fixture) in the physics world and will inform about a collision detection.
    /// </summary>
    public class Mob : SceneObject
    {
        protected readonly IInterestAreaNotifier InterestAreaNotifier;

        private Body body;
        private readonly BodyDefinitionWrapper bodyDefinitionWrapper;

        protected Mob(string name, Vector2 position, Vector2 size) 
            : base(IdGenerator.GetId(), name, new TransformDetails(position, size, Direction.Left))
        {
            InterestAreaNotifier = Components.AddComponent(new InterestAreaNotifier());
            var physicsCollisionNotifier = Components.AddComponent(new PhysicsCollisionNotifier());

            var fixtureDefinition = PhysicsUtils.CreateFixtureDefinition(size, LayerMask.Mob, physicsCollisionNotifier);
            bodyDefinitionWrapper = PhysicsUtils.CreateBodyDefinitionWrapper(fixtureDefinition, position, this);
            bodyDefinitionWrapper.BodyDefiniton.AllowSleep = false;
        }

        protected void CreateBody()
        {
            var presenceSceneProvider = Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            var entityManager = presenceSceneProvider.Scene.Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(new BodyInfo(Id, bodyDefinitionWrapper));
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