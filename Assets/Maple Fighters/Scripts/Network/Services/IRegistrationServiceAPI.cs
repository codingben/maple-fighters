using System.Threading.Tasks;
using CommonTools.Coroutines;
using Registration.Common;

namespace Scripts.Services
{
    public interface IRegistrationServiceAPI : IPeerLogicBase
    {
        Task<RegisterResponseParameters> Register(IYield yield, RegisterRequestParameters parameters);
    }
}