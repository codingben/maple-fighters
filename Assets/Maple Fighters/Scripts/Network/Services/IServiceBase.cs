using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.Utils;

namespace Scripts.Services
{
    public interface IServiceBase : IDisposable
    {
        Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ConnectionInformation connectionInformation);

        void SetNetworkTrafficState(NetworkTrafficState state);

        NetworkTrafficState? NetworkTrafficState();
        bool IsConnected();
    }
}