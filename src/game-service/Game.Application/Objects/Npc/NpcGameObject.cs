using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class NpcGameObject : GameObject
    {
        public NpcGameObject(int id, string name)
            : base(id, name)
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new ProximityChecker());
            Components.Add(new PresenceSceneProvider());
            Components.Add(new NpcConfigDataProvider());
            Components.Add(new NpcIdleBehaviour());
        }
    }
}