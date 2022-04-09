using Game.Application.Objects.Components;
using Game.MessageTools;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id, string name, IJsonSerializer jsonSerializer)
            : base(id, name)
        {
            Components.Add(new GameObjectGetter(this));
            Components.Add(new ProximityChecker());
            Components.Add(new PresenceMapProvider());
            Components.Add(new AnimationData());
            Components.Add(new CharacterData());
            Components.Add(new PresenceMapProvider());
            Components.Add(new MessageSender(jsonSerializer));
            Components.Add(new PositionChangedMessageSender());
            Components.Add(new AnimationStateChangedMessageSender());
            Components.Add(new PlayerAttackedMessageSender());
            Components.Add(new BubbleNotificationMessageSender());
            Components.Add(new InterestManagementNotifier());
            Components.Add(new PhysicsBodyPositionSetter());
        }
    }
}