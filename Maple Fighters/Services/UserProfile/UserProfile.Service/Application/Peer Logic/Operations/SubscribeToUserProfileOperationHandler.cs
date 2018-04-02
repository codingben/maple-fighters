using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class SubscribeToUserProfileOperationHandler : IOperationRequestHandler<SubscribeToUserProfileRequestParameters, EmptyParameters>
    {
        private readonly IUserIdToServerIdConverter userIdToServerIdConverter;

        public SubscribeToUserProfileOperationHandler()
        {
            userIdToServerIdConverter = Server.Components.GetComponent<IUserIdToServerIdConverter>().AssertNotNull();
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