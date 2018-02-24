using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    internal class CreateAuthorizationOperation : IOperationRequestHandler<CreateAuthorizationRequestParameters, CreateAuthorizationResponseParameters>
    {
        public CreateAuthorizationResponseParameters? Handle(MessageData<CreateAuthorizationRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}