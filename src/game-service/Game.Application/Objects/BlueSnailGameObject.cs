using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class BlueSnailGameObject : GameObject
    {
        public BlueSnailGameObject(int id)
            : base(id, name: "BlueSnail")
        {
            Components.Add(new GameObjectGetter(this));
        }

        public void AddProximityChecker(IGameScene gameScene)
        {
            Components.Add(new ProximityChecker(gameScene));
        }
    }
}