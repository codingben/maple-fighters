using Common.ComponentModel;
using Common.MathematicsHelper;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, Vector2 position, IComponent[] components)
            : base(id, name: "Portal")
        {
            Transform.SetPosition(position);
        }
    }
}