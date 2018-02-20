using CharactersService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace CharactersService.Application.PeerLogics.Operations
{
    internal class CreateCharacterOperationHandler : IOperationRequestHandler<CreateCharacterRequestParameters, CreateCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly IDatabaseCharacterCreator databaseCharacterCreator;
        private readonly IDatabaseCharacterExistence databaseCharacterExistence;
        private readonly IDatabaseCharacterNameVerifier databaseCharacterNameVerifier;

        public CreateCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharacterCreator = Server.Components.GetComponent<IDatabaseCharacterCreator>().AssertNotNull();
            databaseCharacterExistence = Server.Components.GetComponent<IDatabaseCharacterExistence>().AssertNotNull();
            databaseCharacterNameVerifier = Server.Components.GetComponent<IDatabaseCharacterNameVerifier>().AssertNotNull();
        }

        public CreateCharacterResponseParameters? Handle(MessageData<CreateCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterClass = messageData.Parameters.CharacterClass;
            var name = messageData.Parameters.Name;
            var characterIndex = messageData.Parameters.Index;

            if (databaseCharacterExistence.Exists(userId, characterIndex))
            {
                return new CreateCharacterResponseParameters(CharacterCreationStatus.Failed);
            }

            if (databaseCharacterNameVerifier.Verify(name))
            {
                return new CreateCharacterResponseParameters(CharacterCreationStatus.NameUsed);
            }

            databaseCharacterCreator.Create(userId, name, characterClass, characterIndex);
            return new CreateCharacterResponseParameters(CharacterCreationStatus.Succeed);
        }
    }
}