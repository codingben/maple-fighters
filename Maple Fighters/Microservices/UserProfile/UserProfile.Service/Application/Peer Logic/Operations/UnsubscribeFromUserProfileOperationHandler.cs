using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    internal class UnsubscribeFromUserProfileOperationHandler : IOperationRequestHandler<UnsubscribeFromUserProfileRequestParameters, EmptyParameters>
    {
        private readonly IUserIdToServerIdConverter userIdToServerIdConverter;

        public UnsubscribeFromUserProfileOperationHandler()
        {
            userIdToServerIdConverter = ServerComponents.GetComponent<IUserIdToServerIdConverter>().AssertNotNull();
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