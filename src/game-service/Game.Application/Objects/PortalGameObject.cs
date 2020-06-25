using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, IGameScene gameScene)
            : base(id, "Portal")
        {
            Components.Add(new PresenceSceneProvider(gameScene));
            Components.Add(new ProximityChecker());
        }

        public void AddPortalData(byte map)
        {
            Components.Add(new PortalData(map));
        }
    }
}