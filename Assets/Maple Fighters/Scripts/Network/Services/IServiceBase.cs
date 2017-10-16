using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Utils;

namespace Scripts.Services
{
    public interface IServiceBase
    {
        Task<ConnectionStatus> Connect(IYield yield, ICoroutinesExecutor coroutinesExecutor, ConnectionInformation connectionInformation);

        void Disconnect();

        bool IsConnected();
    }
}