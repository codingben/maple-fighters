using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Login.Common;
using Scripts.ScriptableObjects;

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

        public bool IsConnected()
        {
            return IsServerConnected();
        }

        public async Task<ConnectionStatus> Connect(IYield yield)
        {
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Login);
            var connectionStatus = await Connect(yield, connectionInformation);
            return connectionStatus;
        }

        public void Disconnect()
        {
            Dispose();
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