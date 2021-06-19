using Common.ComponentModel;
using Common.MathematicsHelper;

namespace Game.Application.Objects
{
    public class GuardianGameObject : GameObject
    {
        public GuardianGameObject(int id, Vector2 position, Vector2 size, IComponent[] components)
            : base(id, name: "Guardian", position, size, components)
        {
            // Left blank intentionally
        }
    }
}