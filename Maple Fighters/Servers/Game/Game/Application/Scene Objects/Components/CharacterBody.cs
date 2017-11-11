using Box2DX.Dynamics;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;

namespace Game.Application.SceneObjects.Components
{
    internal class CharacterBody : Component<ISceneObject>
    {
        private readonly Body body;
        private readonly World world;

        private ITransform transform;

        public CharacterBody(Body body, World world)
        {
            this.body = body;
            this.world = world;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            transform = Entity.Container.GetComponent<ITransform>().AssertNotNull();
            transform.PositionChanged += OnPositionChanged;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            world.DestroyBody(body);
        }

        private void OnPositionChanged(Vector2 position)
        {
            const float TARGET_SPEED = 10.5f; // TODO: Get this data from another source
            const float PHYSICS_SIMULATION_FPS = 60.0f; // TODO: Get this data from another source

            var direction = position - body.GetPosition().ToVector2();
            var distanceToTravel = direction.FromVector2().Normalize();

            var speed = TARGET_SPEED;

            var distancePerTimestep = speed / PHYSICS_SIMULATION_FPS;
            if (distancePerTimestep > distanceToTravel)
            {
                speed *= (distanceToTravel / distancePerTimestep);
            }

            var desiredVelocity = speed * direction;
            var changeInVelocity = desiredVelocity - body.GetLinearVelocity().ToVector2();

            var force = body.GetMass() * PHYSICS_SIMULATION_FPS * changeInVelocity;
            body.ApplyForce(force.FromVector2(), body.GetWorldCenter());
        }
    }
}