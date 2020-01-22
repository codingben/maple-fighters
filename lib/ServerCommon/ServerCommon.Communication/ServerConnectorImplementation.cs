using Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerCommon.Communication
{
    public interface IServerConnectorProvider
    {
        IServerConnector Provide();
    }

    [ComponentSettings(ExposedState.Unexposable)]
    public class ServerConnectorProvider : ComponentBase, IServerConnectorProvider
    {
        private readonly IServerConnector serverConnector;

        public ServerConnectorProvider(IServerConnector serverConnector)
        {
            this.serverConnector = serverConnector;
        }

        public IServerConnector Provide()
        {
            return serverConnector;
        }
    }
}