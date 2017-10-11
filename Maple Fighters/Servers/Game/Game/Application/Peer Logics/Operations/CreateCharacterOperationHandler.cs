using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class CreateCharacterOperationHandler : IOperationRequestHandler<CreateCharacterRequestParameters, CreateCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly DatabaseCharacterCreator databaseCharacterCreator;
        private readonly DatabaseCharacterExistence databaseCharacterExistence;
        private readonly DatabaseCharacterNameVerifier databaseCharacterNameVerifier;

        public CreateCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharacterCreator = Server.Entity.Container.GetComponent<DatabaseCharacterCreator>().AssertNotNull();
            databaseCharacterExistence = Server.Entity.Container.GetComponent<DatabaseCharacterExistence>().AssertNotNull();
            databaseCharacterNameVerifier = Server.Entity.Container.GetComponent<DatabaseCharacterNameVerifier>().AssertNotNull();
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