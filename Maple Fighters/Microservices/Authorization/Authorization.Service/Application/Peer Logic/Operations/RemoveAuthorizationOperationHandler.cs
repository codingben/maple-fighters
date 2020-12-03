using Authorization.Server.Common;
using Authorization.Service.Application.Components.Interfaces;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    internal class RemoveAuthorizationOperationHandler : IOperationRequestHandler<RemoveAuthorizationRequestParameters, EmptyParameters>
    {
        private readonly IAccessTokenRemover accessTokenRemover;

        public RemoveAuthorizationOperationHandler()
        {
            accessTokenRemover = ServerComponents.GetComponent<IAccessTokenRemover>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<RemoveAuthorizationRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            accessTokenRemover.Remove(userId);
            return null;
        }
    }
}