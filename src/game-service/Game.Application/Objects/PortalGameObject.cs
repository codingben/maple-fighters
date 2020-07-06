using Game.Application.Objects.Components;
using InterestManagement;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id)
            : base(id, name: "Portal")
        {
            Components.Add(new GameObjectGetter(this));
        }

        public void AddProximityChecker(IMatrixRegion<IGameObject> matrixRegion)
        {
            var proximityChecker = Components.Add(new ProximityChecker());
            proximityChecker.SetMatrixRegion(matrixRegion);
        }

        public void AddPortalData(byte map)
        {
            Components.Add(new PortalData(map));
        }
    }
}