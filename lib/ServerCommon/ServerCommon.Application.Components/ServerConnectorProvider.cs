using System;
using Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class ServerConnectorProvider : ComponentBase, IServerConnectorProvider
    {
        private readonly IServerConnector serverConnector;

        public ServerConnectorProvider(IServerConnector serverConnector)
        {
            this.serverConnector = serverConnector
                                   ?? throw new ArgumentNullException(
                                       nameof(serverConnector));
        }

        public IServerConnector GetServerConnector()
        {
            return serverConnector;
        }
    }
}