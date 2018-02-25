using Authorization.Server.Common;
using Authorization.Service.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class CreateAuthorizationOperation : IOperationRequestHandler<CreateAuthorizationRequestParameters, CreateAuthorizationResponseParameters>
    {
        private readonly IAccessTokenCreator accessTokenCreator;

        public CreateAuthorizationOperation()
        {
            accessTokenCreator = Server.Components.GetComponent<IAccessTokenCreator>().AssertNotNull();
        }

        public CreateAuthorizationResponseParameters? Handle(MessageData<CreateAuthorizationRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var accessToken = accessTokenCreator.Create(userId);
            return new CreateAuthorizationResponseParameters(accessToken);
        }
    }
}