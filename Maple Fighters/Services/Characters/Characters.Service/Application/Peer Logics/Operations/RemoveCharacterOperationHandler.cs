using CharactersService.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace CharactersService.Application.PeerLogics.Operations
{
    internal class RemoveCharacterOperationHandler : IOperationRequestHandler<RemoveCharacterRequestParameters, RemoveCharacterResponseParameters>
    {
        private readonly int userId;
        private readonly IDatabaseCharacterRemover databaseCharacterRemover;

        public RemoveCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharacterRemover = Server.Components.GetComponent<IDatabaseCharacterRemover>().AssertNotNull();
        }

        public RemoveCharacterResponseParameters? Handle(MessageData<RemoveCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterIndex = messageData.Parameters.CharacterIndex;
            var removed = !databaseCharacterRemover.Remove(userId, characterIndex);
            return new RemoveCharacterResponseParameters(removed ? RemoveCharacterStatus.Succeed : RemoveCharacterStatus.Failed);
        }
    }
}