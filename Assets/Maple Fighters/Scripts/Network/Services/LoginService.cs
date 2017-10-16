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

        public async Task<LoginResponseParameters> Login(IYield yield, LoginRequestParameters parameters)
        {
            var requestId = OperationRequestSender.Send(LoginOperations.Login, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<LoginResponseParameters>(yield, requestId);
            if (responseParameters.HasAccessToken)
            {
                AccessTokenProvider.AccessToken = responseParameters.AccessToken;
            }
            return new LoginResponseParameters(responseParameters.Status);
        }
    }
}