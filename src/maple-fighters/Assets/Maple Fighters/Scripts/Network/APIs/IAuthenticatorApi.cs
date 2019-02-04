using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;
using Registration.Common;

namespace Scripts.Services
{
    public interface IAuthenticatorApi
    {
        Task<AuthenticateResponseParameters> AuthenticateAsync(
            IYield yield,
            AuthenticateRequestParameters parameters);

        Task<RegisterResponseParameters> RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters);
    }
}