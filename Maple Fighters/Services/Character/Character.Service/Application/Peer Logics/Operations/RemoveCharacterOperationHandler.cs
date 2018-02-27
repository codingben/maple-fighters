using Character.Client.Common;
using CharacterService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace CharacterService.Application.PeerLogics.Operations
{
    internal class RemoveCharacterOperationHandler : IOperationRequestHandler<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly IDatabaseCharacterRemover databaseCharacterRemover;
        private readonly IDatabaseCharacterExistence databaseCharacterExistence;

        public RemoveCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharacterRemover = Server.Components.GetComponent<IDatabaseCharacterRemover>().AssertNotNull();
            databaseCharacterExistence = Server.Components.GetComponent<IDatabaseCharacterExistence>().AssertNotNull();
        }

        public RemoveCharacterResponseParameters? Handle(MessageData<RemoveCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterIndex = messageData.Parameters.CharacterIndex;
            databaseCharacterRemover.Remove(userId, characterIndex);

            var removed = !databaseCharacterExistence.Exists(userId, (CharacterIndex)characterIndex);
            return new RemoveCharacterResponseParameters(removed ? RemoveCharacterStatus.Succeed : RemoveCharacterStatus.Failed);
        }
    }
}