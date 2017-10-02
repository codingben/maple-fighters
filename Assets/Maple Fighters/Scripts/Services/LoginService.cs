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

        public void Connect()
        {
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Login);
            Connect(connectionInformation);
        }

        public void Disconnect()
        {
            Dispose();
        }

        public async Task<LoginResponseParameters> Login(IYield yield, LoginRequestParameters parameters)
        {
            var requestId = OperationRequestSender.Send(LoginOperations.Login, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<LoginResponseParameters>(yield, requestId);
            return new LoginResponseParameters(responseParameters.Status);
        }
    }
}