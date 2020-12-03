using Authorization.Client.Common;
using Authorization.Server.Common;
using Authorization.Service.Application.Components.Interfaces;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    internal class UserAuthorizationOperationHandler : IOperationRequestHandler<AuthorizeUserRequestParameters, AuthorizeUserResponseParameters>
    {
        private readonly IAccessTokenCreator accessTokenCreator;
        private readonly IAccessTokenExistence accessTokenExistence;

        public UserAuthorizationOperationHandler()
        {
            accessTokenCreator = ServerComponents.GetComponent<IAccessTokenCreator>().AssertNotNull();
            accessTokenExistence = ServerComponents.GetComponent<IAccessTokenExistence>().AssertNotNull();
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