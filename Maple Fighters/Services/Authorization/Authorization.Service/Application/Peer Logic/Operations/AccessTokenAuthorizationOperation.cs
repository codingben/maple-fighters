using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    internal class AccessTokenAuthorizationOperation : IOperationRequestHandler<AuthorizeAccesTokenRequestParameters, AuthorizeAccessTokenResponseParameters>
    {
        public AuthorizeAccessTokenResponseParameters? Handle(MessageData<AuthorizeAccesTokenRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}