using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    internal class UserAuthorizationOperation : IOperationRequestHandler<AuthorizeUserRequestParameters, AuthorizeUserResponseParameters>
    {
        public AuthorizeUserResponseParameters? Handle(MessageData<AuthorizeUserRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            // TODO: If an user does not exist, so create a new access token and return it
            // TODO: If an user exists, so return authorization failed

            throw new System.NotImplementedException();
        }
    }
}