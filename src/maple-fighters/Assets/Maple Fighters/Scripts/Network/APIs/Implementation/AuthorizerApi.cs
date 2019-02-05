using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;

namespace Scripts.Services
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