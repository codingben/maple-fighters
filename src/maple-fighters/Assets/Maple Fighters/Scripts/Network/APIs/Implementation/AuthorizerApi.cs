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