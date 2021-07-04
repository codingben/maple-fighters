using Common.ComponentModel;
using Game.Physics;

namespace Game.Application.Objects.Components
{
    public class PhysicsBodyPositionSetter : ComponentBase
    {
        private IPhysicsWorldManager physicsWorldManager;
        private IGameObject gameObject;

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
            if (physicsWorldManager == null)
            {
                physicsWorldManager = GetPhysicsWorldManager();
            }

            if (physicsWorldManager != null)
            {
                var bodyId = gameObject.Id;

                if (physicsWorldManager.GetBody(bodyId, out var bodyData))
                {
                    var body = bodyData.Body;
                    if (body != null)
                    {
                        var position =
                            gameObject.Transform.Position.FromVector2();
                        var angle =
                            body.GetAngle();

                        body.SetXForm(position, angle);
                    }
                }
            }
        }

        private IPhysicsWorldManager GetPhysicsWorldManager()
        {
            var presenceMapProvider = Components.Get<IPresenceMapProvider>();
            var gameScene = presenceMapProvider?.GetMap();

            return gameScene?.PhysicsWorldManager;
        }
    }
}