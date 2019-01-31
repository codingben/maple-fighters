using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public sealed class AuthorizationPeerLogic : PeerLogicBase, IAuthorizationPeerLogicAPI
    {
        public async Task<AuthorizeResponseParameters> Authorize(
            IYield yield,
            AuthorizeRequestParameters parameters)
        {
            return 
                await ServerPeerHandler
                    .SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>(
                        yield,
                        AuthorizationOperations.Authorize,
                        parameters,
                        MessageSendOptions.DefaultReliable());
        }
    }
}