using Character.Server.Common;
using CharacterService.Application.Components.Interfaces;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace CharacterService.Application.PeerLogics.Operations
{
    internal class ChangeCharacterMapOperationHandler : IOperationRequestHandler<ChangeCharacterMapParameters, EmptyParameters>
    {
        private readonly IDatabaseCharacterMapUpdater characterMapUpdater;

        public ChangeCharacterMapOperationHandler()
        {
            characterMapUpdater = ServerComponents.GetComponent<IDatabaseCharacterMapUpdater>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<ChangeCharacterMapParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var map = messageData.Parameters.Map;
            characterMapUpdater.Update(userId, (byte)map);
            return null;
        }
    }
}