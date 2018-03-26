using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Registration.Common;

namespace Scripts.Services
{
    public sealed class RegistrationPeerLogic : PeerLogicBase, IRegistrationPeerLogicAPI
    {
        public async Task<RegisterResponseParameters> Register(IYield yield, RegisterRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<RegisterRequestParameters, RegisterResponseParameters>
                (yield, (byte)RegistrationOperations.Register, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}