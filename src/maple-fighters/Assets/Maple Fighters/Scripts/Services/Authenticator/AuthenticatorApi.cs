using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;
using Network.Scripts;
using Registration.Common;

namespace Scripts.Services.Authenticator
{
    internal class AuthenticatorApi : NetworkApi<AuthenticatorOperations, EmptyEventCode>, IAuthenticatorApi
    {
        internal AuthenticatorApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }

        public async Task<AuthenticateResponseParameters> AuthenticateAsync(
            IYield yield,
            AuthenticateRequestParameters parameters)
        {
            var id =
                OperationRequestSender.Send(
                    AuthenticatorOperations.Authenticate,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            var responseParameters =
                await SubscriptionProvider
                    .ProvideSubscription<AuthenticateResponseParameters>(
                        yield,
                        id);

            return 
                new AuthenticateResponseParameters(
                    responseParameters.Status,
                    responseParameters.AccessToken);
        }

        public async Task<RegisterResponseParameters> RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters)
        {
            var id =
                OperationRequestSender.Send(
                    AuthenticatorOperations.Register,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            return
                await SubscriptionProvider
                    .ProvideSubscription<RegisterResponseParameters>(
                        yield,
                        id);
        }
    }
}