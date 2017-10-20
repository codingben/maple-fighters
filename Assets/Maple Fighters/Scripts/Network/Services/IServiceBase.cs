using System;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Utils;

namespace Scripts.Services
{
    public interface IServiceBase : IDisposable
    {
        Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ConnectionInformation connectionInformation);

        bool IsConnected();
    }
}