using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;

namespace Scripts.Services
{
    public sealed class LoginService : ServiceBase<LoginOperations, EmptyEventCode>, ILoginServiceAPI
    {
        public async Task<AuthenticateResponseParameters> Authenticate(IYield yield, AuthenticateRequestParameters parameters)
        {
            var responseParameters = await ServerPeerHandler.SendOperation<AuthenticateRequestParameters, AuthenticateResponseParameters>
                (yield, (byte)LoginOperations.Authenticate, parameters, MessageSendOptions.DefaultReliable());
            if (responseParameters.HasAccessToken)
            {
                AccessTokenProvider.AccessToken = responseParameters.AccessToken;
            }
            return new AuthenticateResponseParameters(responseParameters.Status);
        }
    }
}