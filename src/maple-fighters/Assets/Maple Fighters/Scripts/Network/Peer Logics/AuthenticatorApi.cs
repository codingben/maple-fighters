using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;
using Registration.Common;

namespace Scripts.Services
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

    public class AuthenticatorApi : IAuthenticatorApi
    {
        public ServerPeerHandler<AuthenticatorOperations, EmptyEventCode> ServerPeer
        {
            get;
        }

        public AuthenticatorApi()
        {
            ServerPeer =
                new ServerPeerHandler<AuthenticatorOperations, EmptyEventCode>();
        }

        public async Task<AuthenticateResponseParameters> AuthenticateAsync(
            IYield yield,
            AuthenticateRequestParameters parameters)
        {
            var responseParameters = 
                await ServerPeer
                    .SendOperation<AuthenticateRequestParameters, AuthenticateResponseParameters>(
                        yield,
                        AuthenticatorOperations.Authenticate,
                        parameters,
                        MessageSendOptions.DefaultReliable());

            if (responseParameters.HasAccessToken)
            {
                AccessTokenProvider.AccessToken =
                    responseParameters.AccessToken;
            }

            return new AuthenticateResponseParameters(responseParameters.Status);
        }

        public async Task<RegisterResponseParameters> RegisterAsync(
            IYield yield,
            RegisterRequestParameters parameters)
        {
            return
                await ServerPeer
                    .SendOperation<RegisterRequestParameters, RegisterResponseParameters>(
                        yield,
                        AuthenticatorOperations.Register,
                        parameters,
                        MessageSendOptions.DefaultReliable());
        }
    }
}