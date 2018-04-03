using Authorization.Client.Common;
using Authorization.Server.Common;
using Authorization.Service.Application.Components.Interfaces;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class UserAuthorizationOperationHandler : IOperationRequestHandler<AuthorizeUserRequestParameters, AuthorizeUserResponseParameters>
    {
        private readonly IAccessTokenCreator accessTokenCreator;
        private readonly IAccessTokenExistence accessTokenExistence;

        public UserAuthorizationOperationHandler()
        {
            accessTokenCreator = Server.Components.GetComponent<IAccessTokenCreator>().AssertNotNull();
            accessTokenExistence = Server.Components.GetComponent<IAccessTokenExistence>().AssertNotNull();
        }

        public AuthorizeUserResponseParameters? Handle(MessageData<AuthorizeUserRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            if (accessTokenExistence.Exists(userId))
            {
                return new AuthorizeUserResponseParameters();
            }

            var accessToken = accessTokenCreator.Create(userId);
            return new AuthorizeUserResponseParameters(accessToken, AuthorizationStatus.Succeed);
        }
    }
}