using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    public interface IServerConnectorProvider
    {
        IServerConnector GetServerConnector();
    }
}