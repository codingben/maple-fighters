using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class RemoveCharacterOperationHandler : IOperationRequestHandler<RemoveCharacterRequestParameters, EmptyParameters>
    {
        private readonly int userId;
        private readonly DatabaseCharacterRemover databaseCharacterRemover;

        public RemoveCharacterOperationHandler(int userId)
        {
            this.userId = userId;

            databaseCharacterRemover = Server.Entity.Container.GetComponent<DatabaseCharacterRemover>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<RemoveCharacterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterIndex = messageData.Parameters.CharacterIndex;
            databaseCharacterRemover.Remove(userId, characterIndex);
            return null;
        }
    }
}