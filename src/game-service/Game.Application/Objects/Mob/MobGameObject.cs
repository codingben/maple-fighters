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
            Components.Add(new PresenceSceneProvider());
            Components.Add(new MobMoveBehaviour());
            Components.Add(new MobAttackedBehaviour());
            Components.Add(new MobBehaviourManager());
            Components.Add(new MobAttackPlayerHandler(id));
            Components.Add(new MobConfigDataProvider());
            Components.Add(new MobPhysicsBodyCreator());
            Components.Add(new PhysicsBodyPositionSetter());
        }
    }
}