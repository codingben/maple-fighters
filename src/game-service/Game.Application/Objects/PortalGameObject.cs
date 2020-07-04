using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id)
            : base(id, name: "Portal")
        {
            Components.Add(new GameObjectGetter(this));
        }

        public void AddProximityChecker(IGameScene gameScene)
        {
            Components.Add(new ProximityChecker(gameScene));
        }

        public void AddPortalData(byte map)
        {
            Components.Add(new PortalData(map));
        }
    }
}