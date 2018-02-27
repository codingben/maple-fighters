using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;

namespace Scripts.Services
{
    public sealed class LoginService : ServiceBase<LoginOperations, EmptyEventCode>, ILoginService
    {
        protected override void OnConnected()
        {
            // Left blank intentionally
        }

        protected override void OnDisconnected()
        {
            // Left blank intentionally
        }

        public async Task<AuthenticateResponseParameters> Login(IYield yield, AuthenticateRequestParameters parameters)
        {
            var requestId = OperationRequestSender.Send(LoginOperations.Authenticate, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<AuthenticateResponseParameters>(yield, requestId);
            if (responseParameters.HasAccessToken)
            {
                AccessTokenProvider.AccessToken = responseParameters.AccessToken;
            }
            return new AuthenticateResponseParameters(responseParameters.Status);
        }
    }
}