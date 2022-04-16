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
        }

        protected override void OnRemoved()
        {
            gameObject.Transform.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged()
        {
            var physicsWorldManager = GetPhysicsWorldManager();
            if (physicsWorldManager == null)
            {
                return;
            }

            if (physicsWorldManager.GetBody(gameObject.Id, out var bodyData))
            {
                var body = bodyData.Body;
                if (body != null)
                {
                    body.SetXForm(
                        position: gameObject.Transform.Position.FromVector2(),
                        angle: body.GetAngle());
                }
            }
        }

        private IPhysicsWorldManager GetPhysicsWorldManager()
        {
            if (physicsWorldManager != null)
            {
                return physicsWorldManager;
            }

            var presenceMapProvider = Components.Get<IPresenceMapProvider>();
            var gameScene = presenceMapProvider?.GetMap();
            physicsWorldManager = gameScene?.PhysicsWorldManager;

            return physicsWorldManager;
        }
    }
}