using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Scripts.Network.Core;

namespace Scripts.Network.APIs
{
    public class AuthorizerApi : ApiBase<AuthorizationOperations, EmptyEventCode>, IAuthorizerApi
    {
        public async Task<AuthorizeResponseParameters> AuthorizeAsync(
            IYield yield,
            AuthorizeRequestParameters parameters)
        {
            return 
                await ServerPeerHandler
                    .SendOperationAsync<AuthorizeRequestParameters, AuthorizeResponseParameters>(
                        yield,
                        AuthorizationOperations.Authorize,
                        parameters,
                        MessageSendOptions.DefaultReliable());
        }
    }
}