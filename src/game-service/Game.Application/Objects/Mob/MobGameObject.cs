using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class MobGameObject : GameObject
    {
        public MobGameObject(int id, string name)
            : base(id, name)
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new ProximityChecker());
            Components.Add(new PresenceMapProvider());
            Components.Add(new MobConfigDataProvider());
            Components.Add(new MobPhysicsBodyCreator());
            Components.Add(new MobMoveBehaviour());
            Components.Add(new PhysicsBodyPositionSetter());
        }
    }
}