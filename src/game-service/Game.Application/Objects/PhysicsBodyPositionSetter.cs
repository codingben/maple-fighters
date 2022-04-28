using Game.Application.Components;
using Game.Physics;

namespace Game.Application.Objects.Components
{
    public class PhysicsBodyPositionSetter : ComponentBase
    {
        private IGameObject gameObject;
        private IPhysicsWorldManager physicsWorldManager;

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();

            gameObject = gameObjectGetter.Get();
            gameObject.Transform.PositionChanged += OnPositionChanged;

            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            presenceSceneProvider.SceneChanged += (gameScene) =>
            {
                var gamePhysicsCreator =
                    gameScene.Components.Get<IScenePhysicsCreator>();
                physicsWorldManager =
                    gamePhysicsCreator.GetPhysicsWorldManager();
            };
        }

        protected override void OnRemoved()
        {
            gameObject.Transform.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged()
        {
            if (physicsWorldManager == null)
            {
                return;
            }

            if (physicsWorldManager.GetBody(gameObject.Id, out var bodyData))
            {
                var body = bodyData.Body;
                if (body != null)
                {
                    var position = gameObject.Transform.Position.FromVector2();
                    var angle = body.GetAngle();

                    body.SetXForm(position, angle);
                }
            }
        }
    }
}