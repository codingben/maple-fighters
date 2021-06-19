using Common.ComponentModel;
using Common.MathematicsHelper;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, Vector2 position, Vector2 size, IComponent[] components)
            : base(id, name: "Portal", position, size, components)
        {
            Transform.SetPosition(position);
        }
    }
}