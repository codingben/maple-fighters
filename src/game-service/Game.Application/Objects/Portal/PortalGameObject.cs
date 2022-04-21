using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PortalGameObject : GameObject
    {
        public PortalGameObject(int id, string name, string customData)
            : base(id, name)
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new ProximityChecker());
            Components.Add(new PresenceMapProvider());
            Components.Add(new PortalData(map: byte.Parse(customData)));
        }
    }
}