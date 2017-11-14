using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
using Shared.Game.Common;

namespace Game.Application.SceneObjects.Components
{
    internal class CharacterBody : Component<ISceneObject>, ICharacterBody
    {
        public PlayerState PlayerState { get; set; }

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
            switch (PlayerState)
            {
                case PlayerState.Idle:
                case PlayerState.Moving:
                {
                    if (body.GetMass() == 0)
                    {
                        body.SetMassFromShapes();
                        return;
                    }

                    const float SPEED = 10.5f; // TODO: Get this data from another source
                    body.MoveBody(position, SPEED);
                    break;
                }
                case PlayerState.Falling:
                case PlayerState.Rope:
                case PlayerState.Ladder:
                {
                    if (body.GetMass() > 0)
                    {
                        body.SetMass(new MassData());
                        return;
                    }

                    body.SetXForm(position.FromVector2(), body.GetAngle());
                    break;
                }
            }
        }
    }
}