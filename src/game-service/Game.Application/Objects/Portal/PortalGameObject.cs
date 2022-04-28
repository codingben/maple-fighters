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
            Components.Add(new PresenceSceneProvider());
            Components.Add(new PortalTeleportationData(map: byte.Parse(customData)));
        }
    }
}