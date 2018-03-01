using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public interface IServiceConnectionHandler : IDisposable
    {
        ServerConnectionInformation ServerConnectionInformation { get; }
        IPeerDisconnectionNotifier PeerDisconnectionNotifier { get; }

        Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ServerConnectionInformation serverConnectionInformation);
        void SetNetworkTrafficState(NetworkTrafficState state);

        bool IsConnected();
    }
}