using Game.Application.Messages;
using Game.Application.Network;
using Game.Application.Objects.Components;

namespace Game.Application.Handlers
{
    public class EnterSceneMessageHandler : IMessageHandler
    {
        private ICharacterData characterData;

        public EnterSceneMessageHandler(ICharacterData characterData)
        {
            this.characterData = characterData;
        }

        public void Handle(byte[] rawData)
        {
            var message =
                MessageUtils.DeserializeMessage<EnterSceneMessage>(rawData);
            var name = message.Character.Name;
            var characterType = message.Character.CharacterType;

            characterData.SetName(name);
            characterData.SetCharacterType(characterType);

            // TODO: Send the entered scene message from here or from component (and get player spawn data)
        }
    }
}