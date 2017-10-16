using System.Threading.Tasks;
using CommonTools.Coroutines;
using Registration.Common;

namespace Scripts.Services
{
    public interface IRegistrationService : IServiceBase
    {
        Task<RegisterResponseParameters> Register(IYield yield, RegisterRequestParameters parameters);
    }
}