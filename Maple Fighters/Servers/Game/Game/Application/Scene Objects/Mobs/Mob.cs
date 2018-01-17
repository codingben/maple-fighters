using Box2DX.Dynamics;
using CommonTools.Log;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
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

        protected Mob(string name, Vector2 position, Vector2 bodySize) 
            : base(name, position, 1)
        {
            InterestAreaNotifier = Container.AddComponent(new InterestAreaNotifier());
            var physicsCollisionNotifier = Container.AddComponent(new PhysicsCollisionNotifier());

            var fixtureDefinition = PhysicsUtils.CreateFixtureDefinition(bodySize, LayerMask.Mob, physicsCollisionNotifier);
            bodyDefinitionWrapper = PhysicsUtils.CreateBodyDefinitionWrapper(fixtureDefinition, position, this);
            bodyDefinitionWrapper.BodyDef.AllowSleep = false;
        }

        protected void CreateBody()
        {
            var presenceScene = Container.GetComponent<IPresenceScene>().AssertNotNull();
            var entityManager = presenceScene.Scene.Entity.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(new BodyInfo(Id, bodyDefinitionWrapper));
        }

        protected Body GetBody()
        {
            if (body != null)
            {
                return body;
            }

            var presenceScene = Container.GetComponent<IPresenceScene>().AssertNotNull();
            var entityManager = presenceScene.Scene.Entity.GetComponent<IEntityManager>().AssertNotNull();
            body = entityManager.GetBody(Id).AssertNotNull();
            return body;
        }
    }
}