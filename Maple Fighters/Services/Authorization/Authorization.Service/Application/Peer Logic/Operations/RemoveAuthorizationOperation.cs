using Authorization.Server.Common;
using Authorization.Service.Application.Components;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;

namespace Authorization.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class RemoveAuthorizationOperation : IOperationRequestHandler<RemoveAuthorizationRequestParameters, EmptyParameters>
    {
        private readonly IAccessTokenRemover accessTokenRemover;

        public RemoveAuthorizationOperation()
        {
            accessTokenRemover = Server.Components.GetComponent<IAccessTokenRemover>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<RemoveAuthorizationRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            accessTokenRemover.Remove(userId);
            return null;
        }
    }
}