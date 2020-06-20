using Common.MathematicsHelper;
using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(
            int id,
            Vector2 position,
            IGameScene scene,
            byte map)
            : base(id, "Portal")
        {
            Transform.SetPosition(position);
            Transform.SetSize(Vector2.One);

            Components.Add(new PresenceSceneProvider(scene));
            Components.Add(new ProximityChecker());
            Components.Add(new PortalData(map));
        }
    }
}