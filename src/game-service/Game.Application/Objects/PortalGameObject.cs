using Common.MathematicsHelper;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, Vector2 position)
            : base(id, name: "Portal")
        {
            Transform.SetPosition(position);
        }

        public void AddPortalData(byte map)
        {
            Components.Add(new PortalData(map));
        }
    }
}