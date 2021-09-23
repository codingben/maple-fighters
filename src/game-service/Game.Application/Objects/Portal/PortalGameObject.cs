using Common.ComponentModel;
using Common.MathematicsHelper;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, Vector2 position, IComponent[] components)
            : base(id, name: "Portal", position, size: new Vector2(10, 5), components)
        {
            Transform.SetPosition(position);
        }
    }
}