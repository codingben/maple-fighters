using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class UnsubscribeFromUserProfileOperationHandler : IOperationRequestHandler<UnsubscribeFromUserProfileRequestParameters, EmptyParameters>
    {
        private readonly IUserIdToServerIdConverter userIdToServerIdConverter;

        public UnsubscribeFromUserProfileOperationHandler()
        {
            userIdToServerIdConverter = Server.Components.GetComponent<IUserIdToServerIdConverter>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<UnsubscribeFromUserProfileRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var serverId = messageData.Parameters.ServerId;
            userIdToServerIdConverter.Remove(userId, serverId);
            return null;
        }
    }
}