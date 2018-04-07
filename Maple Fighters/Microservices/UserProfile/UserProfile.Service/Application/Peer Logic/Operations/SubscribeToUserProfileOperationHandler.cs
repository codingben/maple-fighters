using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    internal class SubscribeToUserProfileOperationHandler : IOperationRequestHandler<SubscribeToUserProfileRequestParameters, EmptyParameters>
    {
        private readonly IUserIdToServerIdConverter userIdToServerIdConverter;

        public SubscribeToUserProfileOperationHandler()
        {
            userIdToServerIdConverter = ServerComponents.GetComponent<IUserIdToServerIdConverter>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<SubscribeToUserProfileRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;
            var serverId = messageData.Parameters.ServerId;
            userIdToServerIdConverter.Add(userId, serverId);
            return null;
        }
    }
}