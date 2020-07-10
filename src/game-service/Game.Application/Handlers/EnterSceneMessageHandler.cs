using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class EnterSceneMessageHandler : IMessageHandler
    {
        private readonly IGameObject player;
        private readonly ICharacterData characterData;
        private readonly IMessageSender messageSender;

        public EnterSceneMessageHandler(IGameObject player)
        {
            this.player = player;
            this.characterData = player.Components.Get<CharacterData>();
            this.messageSender = player.Components.Get<MessageSender>();
        }

        public void Handle(byte[] rawData)
        {
            var message =
                MessageUtils.DeserializeMessage<EnterSceneMessage>(rawData);
            var name = message.CharacterName;
            var characterType = message.CharacterType;

            characterData.SetName(name);
            characterData.SetCharacterType(characterType);

            // TODO: Add to game scene
            // TODO: Set matrix region

            SendEnteredSceneMessage();
        }

        private void SendEnteredSceneMessage()
        {
            var messageCode = (byte)MessageCodes.EnteredScene;
            var message = new EnteredSceneMessage()
            {
                GameObjectId = player.Id,
                SpawnPositionData = new SpawnPositionData
                {
                    X = player.Transform.Position.X,
                    Y = player.Transform.Position.Y
                }
            };

            messageSender.SendMessage(messageCode, message);
        }
    }
}