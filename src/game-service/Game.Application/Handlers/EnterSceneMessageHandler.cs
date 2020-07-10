using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class EnterSceneMessageHandler : IMessageHandler
    {
        private readonly IGameObjectGetter gameObjectGetter;
        private readonly ICharacterData characterData;
        private readonly IMessageSender messageSender;

        public EnterSceneMessageHandler(
            IGameObjectGetter gameObjectGetter,
            ICharacterData characterData,
            IMessageSender messageSender)
        {
            this.gameObjectGetter = gameObjectGetter;
            this.characterData = characterData;
            this.messageSender = messageSender;
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
            var gameObject = gameObjectGetter.Get();

            messageSender.SendMessage((byte)MessageCodes.EnteredScene, new EnteredSceneMessage()
            {
                GameObjectId = gameObject.Id,
                SpawnPositionData = new SpawnPositionData
                {
                    X = gameObject.Transform.Position.X,
                    Y = gameObject.Transform.Position.Y
                }
            });
        }
    }
}