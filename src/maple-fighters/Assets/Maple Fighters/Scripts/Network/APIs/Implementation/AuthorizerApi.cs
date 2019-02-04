using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;

namespace Scripts.Services
{
    public class AuthorizerApi : IAuthorizerApi
    {
        public ServerPeerHandler<AuthorizationOperations, EmptyEventCode> ServerPeer
        {
            get;
        }

        public AuthorizerApi()
        {
            ServerPeer =
                new ServerPeerHandler<AuthorizationOperations, EmptyEventCode>();
        }

        public async Task<AuthorizeResponseParameters> AuthorizeAsync(
            IYield yield,
            AuthorizeRequestParameters parameters)
        {
            return 
                await ServerPeer
                    .SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>(
                        yield,
                        AuthorizationOperations.Authorize,
                        parameters,
                        MessageSendOptions.DefaultReliable());
        }
    }
}