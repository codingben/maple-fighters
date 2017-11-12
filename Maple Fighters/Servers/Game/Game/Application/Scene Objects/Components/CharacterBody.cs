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

            var transform = Entity?.Container?.GetComponent<ITransform>();
            if (transform != null)
            {
                transform.PositionChanged -= OnPositionChanged;
            }

            world.DestroyBody(body);
        }

        private void OnPositionChanged(Vector2 position)
        {
            const float SPEED = 10.5f; // TODO: Get this data from another source
            body.MoveBody(position, SPEED);
        }
    }
}