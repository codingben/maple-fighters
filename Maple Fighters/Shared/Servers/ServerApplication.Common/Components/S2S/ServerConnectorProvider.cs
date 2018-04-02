using ComponentModel.Common;
using ServerApplication.Common.Components.Interfaces;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class ServerConnectorProvider : Component, IServerConnectorProvider
    {
        private readonly IServerConnector serverConnector;

        public ServerConnectorProvider(IServerConnector serverConnector)
        {
            this.serverConnector = serverConnector;
        }

        public IServerConnector GetServerConnector()
        {
            return serverConnector;
        }
    }
}