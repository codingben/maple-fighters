using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;

namespace Scripts.Services
{
    public interface ILoginService
    {
        bool IsConnected();

        Task<ConnectionStatus> Connect(IYield yield);
        void Disconnect();

        Task<LoginResponseParameters> Login(IYield yield, LoginRequestParameters parameters);
    }
}