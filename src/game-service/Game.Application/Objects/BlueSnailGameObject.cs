using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class BlueSnailGameObject : GameObject
    {
        public BlueSnailGameObject(int id, IGameScene gameScene)
            : base(id, "BlueSnail")
        {
            Components.Add(new ProximityChecker(gameScene));
        }
    }
}