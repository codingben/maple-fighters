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

        NetworkTrafficState? NetworkTrafficState();
        bool IsConnected();
    }
}