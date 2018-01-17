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
        private readonly IDatabaseCharacterCreator databaseCharacterCreator;
        private readonly IDatabaseCharacterExistence databaseCharacterExistence;
        private readonly IDatabaseCharacterNameVerifier databaseCharacterNameVerifier;

        public CreateCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharacterCreator = Server.Entity.GetComponent<IDatabaseCharacterCreator>().AssertNotNull();
            databaseCharacterExistence = Server.Entity.GetComponent<IDatabaseCharacterExistence>().AssertNotNull();
            databaseCharacterNameVerifier = Server.Entity.GetComponent<IDatabaseCharacterNameVerifier>().AssertNotNull();
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