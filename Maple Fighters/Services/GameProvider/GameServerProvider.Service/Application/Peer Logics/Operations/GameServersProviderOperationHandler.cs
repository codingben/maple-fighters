using System;
using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using GameServerProvider.Client.Common;
using GameServerProvider.Service.Application.Components;
using ServerCommunicationHelper;

namespace GameServerProvider.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class GameServersProviderOperationHandler : IOperationRequestHandler<EmptyParameters, GameServersProviderResponseParameters>
    {
        private readonly Action gameServersSent;
        private readonly IGameServersInformationProvider gameServersInformationProvider;

        public GameServersProviderOperationHandler(Action gameServersSent)
        {
            this.gameServersSent = gameServersSent;

            gameServersInformationProvider = Server.Components.GetComponent<IGameServersInformationProvider>().AssertNotNull();
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

            gameServersSent.Invoke();
            return new GameServersProviderResponseParameters(gameServersInformation.ToArray());
        }
    }
}