using Common.MathematicsHelper;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id, Vector2 position)
            : base(id, name: "Player")
        {
            Transform.SetPosition(position);

            AddComponents();
        }

        private void AddComponents()
        {
            Components.Add(new AnimationData());
            // Components.Add(new MessageSender(SendMessageToMySession, SendMessageToSession));
            Components.Add(new PositionChangedMessageSender());
            Components.Add(new AnimationStateChangedMessageSender());
            Components.Add(new CharacterData());
        }
    }
}