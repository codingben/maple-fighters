using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public interface IServerConnectorProvider : IExposableComponent
    {
        IServerConnector GetServerConnector();
    }
}