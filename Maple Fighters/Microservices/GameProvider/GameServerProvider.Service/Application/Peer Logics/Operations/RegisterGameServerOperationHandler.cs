using CommonCommunicationInterfaces;
using CommonTools.Log;
using GameServerProvider.Server.Common;
using GameServerProvider.Service.Application.Components;
using GameServerProvider.Service.Application.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace GameServerProvider.Service.Application.PeerLogic.Operations
{
    internal class RegisterGameServerOperationHandler : IOperationRequestHandler<RegisterGameServerRequestParameters, EmptyParameters>
    {
        private readonly int peerId;
        private readonly IGameServerInformationCreator gameServerInformationCreator;

        public RegisterGameServerOperationHandler(int peerId)
        {
            this.peerId = peerId;

            gameServerInformationCreator = ServerComponents.GetComponent<IGameServerInformationCreator>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<RegisterGameServerRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var parameters = Utils.FromGameServerInformationParameters(messageData.Parameters);
            gameServerInformationCreator.Add(peerId, parameters);
            return null;
        }
    }
}