using Common.MathematicsHelper;
using Game.Application.Objects.Components;
using InterestManagement;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, Vector2 position, IMatrixRegion<IGameObject> region)
            : base(id, name: "Portal", region)
        {
            Transform.SetPosition(position);
        }

        public void AddPortalData(byte map)
        {
            Components.Add(new PortalData(map));
        }
    }
}