using CommonCommunicationInterfaces;
using ComponentModel.Common;

namespace ServerApplication.Common.Components
{
    public interface IServiceBase : IExposableComponent
    {
        bool IsConnected();

        PeerConnectionInformation GetPeerConnectionInformation();
    }
}