using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public interface IServerConnectorProvider
    {
        IServerConnector GetServerConnector();
    }
}