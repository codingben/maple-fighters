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

        public void AddProximityChecker()
        {
            Components.Add(new ProximityChecker());
        }
    }
}