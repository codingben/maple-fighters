using Authorization.Server.Common;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    internal class RemoveAuthorizationOperation : IOperationRequestHandler<RemoveAuthorizationRequestParameters, EmptyParameters>
    {
        public EmptyParameters? Handle(MessageData<RemoveAuthorizationRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            throw new System.NotImplementedException();
        }
    }
}