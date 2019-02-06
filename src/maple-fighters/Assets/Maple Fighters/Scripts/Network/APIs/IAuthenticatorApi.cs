using System.Threading.Tasks;
using CommonTools.Coroutines;
using Login.Common;
using Registration.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public interface IAuthenticatorApi : IApiBase
    {
        Task<AuthenticateResponseParameters> AuthenticateAsync(
            IYield yield,
            AuthenticateRequestParameters parameters);

        Task<RegisterResponseParameters> RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters);
    }
}