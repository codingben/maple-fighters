using System.Threading.Tasks;
using CommonTools.Coroutines;
using Registration.Common;

namespace Scripts.Services
{
    public interface IRegistrationService
    {
        bool IsConnected();

        Task<ConnectionStatus> Connect(IYield yield);
        void Disconnect();

        Task<RegisterResponseParameters> Register(IYield yield, RegisterRequestParameters parameters);
    }
}