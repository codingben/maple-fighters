using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;

namespace Scripts.Services
{
    public interface ILoginService
    {
        bool IsConnected();

        void Connect();
        void Disconnect();

        Task<LoginResponseParameters> Login(IYield yield, LoginRequestParameters parameters);
    }
}