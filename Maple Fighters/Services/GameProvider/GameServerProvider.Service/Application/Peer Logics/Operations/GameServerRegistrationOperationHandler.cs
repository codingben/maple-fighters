using CommonCommunicationInterfaces;
using CommonTools.Log;
using GameServerProvider.Server.Common;
using GameServerProvider.Service.Application.Components;
using ServerCommunicationHelper;

namespace GameServerProvider.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class GameServerRegistrationOperationHandler : IOperationRequestHandler<RegisterGameServerRequestParameters, EmptyParameters>
    {
        private readonly int peerId;
        private readonly IGameServerInformationCreator gameServerInformationCreator;

        public GameServerRegistrationOperationHandler(int peerId)
        {
            this.peerId = peerId;

            gameServerInformationCreator = Server.Components.GetComponent<IGameServerInformationCreator>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<RegisterGameServerRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var parameters = Utils.FromGameServerInformationParameters(messageData.Parameters);
            gameServerInformationCreator.Add(peerId, parameters);
            return null;
        }
    }
}