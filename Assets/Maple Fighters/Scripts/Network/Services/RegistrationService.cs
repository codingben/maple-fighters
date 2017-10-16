using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Registration.Common;

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

        public async Task<RegisterResponseParameters> Register(IYield yield, RegisterRequestParameters parameters)
        {
            var requestId = OperationRequestSender.Send(RegistrationOperations.Register, parameters, MessageSendOptions.DefaultReliable());
            var responseParameters = await SubscriptionProvider.ProvideSubscription<RegisterResponseParameters>(yield, requestId);
            return new RegisterResponseParameters(responseParameters.Status);
        }
    }
}