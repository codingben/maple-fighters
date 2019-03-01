using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;
using Registration.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public class DummyAuthenticatorApi : ApiBase<AuthenticatorOperations, EmptyEventCode>, IAuthenticatorApi
    {
        public Task<AuthenticateResponseParameters> AuthenticateAsync(
            IYield yield,
            AuthenticateRequestParameters parameters)
        {
            return Task.FromResult(
                new AuthenticateResponseParameters(LoginStatus.Succeed));
        }

        public Task<RegisterResponseParameters> RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters)
        {
            return Task.FromResult(
                new RegisterResponseParameters(RegisterStatus.Succeed));
        }
    }
}