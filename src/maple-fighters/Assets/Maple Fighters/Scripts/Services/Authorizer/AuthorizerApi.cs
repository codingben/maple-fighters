using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using Network.Scripts;

namespace Scripts.Services.Authorizer
{
    internal class AuthorizerApi : NetworkApi<AuthorizationOperations, EmptyEventCode>, IAuthorizerApi
    {
        internal AuthorizerApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            // Left blank intentionally
        }

        public async Task<AuthorizeResponseParameters> AuthorizeAsync(
            IYield yield,
            AuthorizeRequestParameters parameters)
        {
            var id =
                OperationRequestSender.Send(
                    AuthorizationOperations.Authorize,
                    parameters,
                    MessageSendOptions.DefaultReliable());

            return
                await SubscriptionProvider
                    .ProvideSubscription<AuthorizeResponseParameters>(
                        yield,
                        id);
        }
    }
}