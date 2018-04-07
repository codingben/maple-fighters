using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using GameServerProvider.Client.Common;
using GameServerProvider.Service.Application.Components.Interfaces;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace GameServerProvider.Service.Application.PeerLogic.Operations
{
    internal class GameServersProviderOperationHandler : IOperationRequestHandler<EmptyParameters, GameServersProviderResponseParameters>
    {
        private readonly IGameServersInformationProvider gameServersInformationProvider;

        public GameServersProviderOperationHandler()
        {
            gameServersInformationProvider = ServerComponents.GetComponent<IGameServersInformationProvider>().AssertNotNull();
        }

        public GameServersProviderResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var gameServersInformation = new List<GameServerInformationParameters>();

            foreach (var gameServerInformation in gameServersInformationProvider.Provide())
            {
                var name = gameServerInformation.Name;
                var ip = gameServerInformation.IP;
                var port = gameServerInformation.Port;
                var connections = gameServerInformation.Connections;
                var maxConnections = gameServerInformation.MaxConnections;
                gameServersInformation.Add(new GameServerInformationParameters(name, ip, port, connections, maxConnections));
            }

            return new GameServersProviderResponseParameters(gameServersInformation.ToArray());
        }
    }
}