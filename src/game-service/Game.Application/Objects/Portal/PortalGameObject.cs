using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, string name)
            : base(id, name)
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new ProximityChecker());
            Components.Add(new PresenceMapProvider());
            Components.Add(new PortalData(map: (byte)Map.TheDarkForest));
        }
    }
}