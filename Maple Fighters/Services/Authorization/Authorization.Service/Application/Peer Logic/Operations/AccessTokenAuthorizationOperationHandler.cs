using Authorization.Client.Common;
using Authorization.Server.Common;
using Authorization.Service.Application.Components.Interfaces;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class AccessTokenAuthorizationOperationHandler : IOperationRequestHandler<AuthorizeAccesTokenRequestParameters, AuthorizeAccessTokenResponseParameters>
    {
        private readonly IAccessTokenExistence accessTokenExistence;
        private readonly IAccessTokenGetter accessTokenGetter;

        public AccessTokenAuthorizationOperationHandler()
        {
            accessTokenExistence = Server.Components.GetComponent<IAccessTokenExistence>().AssertNotNull();
            accessTokenGetter = Server.Components.GetComponent<IAccessTokenGetter>().AssertNotNull();
        }

        public AuthorizeAccessTokenResponseParameters? Handle(MessageData<AuthorizeAccesTokenRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var accessToken = messageData.Parameters.AccessToken;
            var isAccessTokenExists = accessTokenExistence.Exists(accessToken);
            if (isAccessTokenExists)
            {
                var userId = accessTokenGetter.Get(accessToken);
                if (userId.HasValue)
                {
                    return new AuthorizeAccessTokenResponseParameters(userId.Value, AuthorizationStatus.Succeed);
                }
            }

            return new AuthorizeAccessTokenResponseParameters();
        }
    }
}