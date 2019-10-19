using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Network.Scripts;

namespace Scripts.Services.Authorizer
{
    internal class DummyAuthorizerApi : NetworkApi<AuthorizationOperations, EmptyEventCode>, IAuthorizerApi
    {
        internal DummyAuthorizerApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }

        public async Task<AuthorizeResponseParameters> AuthorizeAsync(
            IYield yield,
            AuthorizeRequestParameters parameters)
        {
            return
                await Task.FromResult(
                    new AuthorizeResponseParameters(
                        default,
                        AuthorizationStatus.Succeed));
        }
    }
}