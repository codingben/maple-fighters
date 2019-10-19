using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;
using Network.Scripts;
using Registration.Common;

namespace Scripts.Services.Authenticator
{
    internal class DummyAuthenticatorApi : NetworkApi<AuthenticatorOperations, EmptyEventCode>, IAuthenticatorApi
    {
        protected DummyAuthenticatorApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }

        public async Task<AuthenticateResponseParameters> AuthenticateAsync(
            IYield yield,
            AuthenticateRequestParameters parameters)
        {
            return 
                await Task.FromResult(
                    new AuthenticateResponseParameters(LoginStatus.Succeed));
        }

        public async Task<RegisterResponseParameters> RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters)
        {
            return 
                await Task.FromResult(
                    new RegisterResponseParameters(RegisterStatus.Succeed));
        }
    }
}