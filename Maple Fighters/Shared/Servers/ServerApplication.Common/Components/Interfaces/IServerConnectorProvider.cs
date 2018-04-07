using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components.Interfaces
{
    public interface IServerConnectorProvider
    {
        IServerConnector GetServerConnector();
    }
}