using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Registration.Common;
using Scripts.ScriptableObjects;

namespace Scripts.Services
{
    public sealed class RegistrationService : ServiceBase<RegistrationOperations, EmptyEventCode>, IRegistrationService
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
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Registration);
            var connectionStatus = await Connect(yield, connectionInformation);
            return connectionStatus;
        }

        public void Disconnect()
        {
            Dispose();
        }

        public async Task<RegisterResponseParameters> Register(IYield yield, RegisterRequestParameters parameters)
        {
            var requestId = OperationRequestSender.Send(RegistrationOperations.Register, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<RegisterResponseParameters>(yield, requestId);
            return new RegisterResponseParameters(responseParameters.Status);
        }
    }
}