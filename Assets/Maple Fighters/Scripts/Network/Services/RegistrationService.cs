using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Registration.Common;

namespace Scripts.Services
{
    public sealed class RegistrationService : ServiceBase<RegistrationOperations, EmptyEventCode>, IRegistrationService
    {
        public async Task<RegisterResponseParameters> Register(IYield yield, RegisterRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<RegisterRequestParameters, RegisterResponseParameters>
                (yield, (byte)RegistrationOperations.Register, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}