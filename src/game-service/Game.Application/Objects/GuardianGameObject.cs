using Common.ComponentModel;
using Common.MathematicsHelper;

namespace Game.Application.Objects
{
    public class GuardianGameObject : GameObject
    {
        public GuardianGameObject(int id, Vector2 position, IComponent[] components)
            : base(id, name: "Guardian", components)
        {
            Transform.SetPosition(position);
        }
    }
}