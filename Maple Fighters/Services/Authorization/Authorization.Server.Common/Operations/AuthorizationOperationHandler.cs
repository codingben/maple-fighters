using System;
using System.Threading.Tasks;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommunicationHelper;

namespace Authorization.Server.Common
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class AuthorizationOperationHandler : IAsyncOperationRequestHandler<AuthorizeRequestParameters, AuthorizeResponseParameters>
    {
        private readonly Action onAuthorized;
        private readonly Action<int> onAuthorizedArg;
        private readonly Action onNonAuthorized;
        private readonly IAuthorizationServiceAPI authorizationServiceApi;

        public AuthorizationOperationHandler(Action onAuthorized, Action onNonAuthorized)
        {
            this.onAuthorized = onAuthorized;
            this.onNonAuthorized = onNonAuthorized;

            authorizationServiceApi = Server.Components.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
        }

        public AuthorizationOperationHandler(Action<int> onAuthorizedArg, Action onNonAuthorized)
        {
            this.onAuthorizedArg = onAuthorizedArg;
            this.onNonAuthorized = onNonAuthorized;

            authorizationServiceApi = Server.Components.GetComponent<IAuthorizationServiceAPI>().AssertNotNull();
        }

        public Task<AuthorizeResponseParameters?> Handle(IYield yield, MessageData<AuthorizeRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var parameters = new AuthorizeAccesTokenRequestParameters(messageData.Parameters.AccessToken);
            return Authorize(yield, parameters);
        }

        private async Task<AuthorizeResponseParameters?> Authorize(IYield yield, AuthorizeAccesTokenRequestParameters parameters)
        {
            var authorization = await authorizationServiceApi.SendYieldOperation<AuthorizeAccesTokenRequestParameters, AuthorizeAccessTokenResponseParameters>
                (yield, (byte)ServerOperations.AccessTokenAuthorization, parameters);

            if (authorization.Status == AuthorizationStatus.Succeed)
            {
                onAuthorized?.Invoke();
                onAuthorizedArg?.Invoke(authorization.UserId);
            }
            else
            {
                onNonAuthorized?.Invoke();
            }
            return new AuthorizeResponseParameters(authorization.UserId, authorization.Status);
        }
    }
}