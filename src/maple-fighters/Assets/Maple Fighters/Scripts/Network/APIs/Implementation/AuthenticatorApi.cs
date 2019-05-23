using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;
using Registration.Common;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public enum AuthenticatorOperations
    {
        /// <summary>
        /// The authenticate.
        /// </summary>
        Authenticate,

        /// <summary>
        /// The register.
        /// </summary>
        Register
    }

    public class AuthenticatorApi : ApiBase<AuthenticatorOperations, EmptyEventCode>, IAuthenticatorApi
    {
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

            if (responseParameters.HasAccessToken)
            {
                // TODO: Use the access token
            }

            return new AuthenticateResponseParameters(
                responseParameters.Status);
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