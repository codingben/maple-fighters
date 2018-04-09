using CommonCommunicationInterfaces;
using CommonTools.Log;
using GameServerProvider.Server.Common;
using GameServerProvider.Service.Application.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace GameServerProvider.Service.Application.PeerLogic.Operations
{
    internal class UpdateGameServerConnectionsOperationHandler : IOperationRequestHandler<UpdateGameServerConnectionsInfoRequestParameters, EmptyParameters>
    {
        private readonly int peerId;
        private readonly IUpdateGameServerConnectionsInfo updateGameServerConnectionsInfo;

        public UpdateGameServerConnectionsOperationHandler(int peerId)
        {
            this.peerId = peerId;

            updateGameServerConnectionsInfo = ServerComponents.GetComponent<IUpdateGameServerConnectionsInfo>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<UpdateGameServerConnectionsInfoRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var connections = messageData.Parameters.Connections;
            updateGameServerConnectionsInfo.Update(peerId, connections);
            return null;
        }
    }
}