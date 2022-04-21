using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id, string name)
            : base(id, name)
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new ProximityChecker());
            Components.Add(new PresenceMapProvider());
            Components.Add(new PlayerConfigDataProvider());
            Components.Add(new AnimationStateProvider());
            Components.Add(new MessageSender());
            Components.Add(new PositionChangedMessageSender());
            Components.Add(new AnimationStateChangedMessageSender());
            Components.Add(new PlayerAttackedMessageSender());
            Components.Add(new BubbleNotificationMessageSender());
            Components.Add(new InterestManagementNotifier());
            Components.Add(new PlayerPhysicsBodyCreator());
            Components.Add(new PhysicsBodyPositionSetter());
        }
    }
}