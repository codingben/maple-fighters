using ServerCommunicationInterfaces;

namespace ServerCommon.Communication
{
    public interface IServerConnectorProvider
    {
        IServerConnector Provide();
    }
}