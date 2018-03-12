using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class ChangeUserProfilePropertiesOperationHandler : IOperationRequestHandler<ChangeUserProfilePropertiesRequestParameters, EmptyParameters>
    {
        private readonly IDatabaseUserProfileExistence databaseUserProfileExistence;
        private readonly IDatabaseUserProfilePropertiesUpdater databaseUserProfilePropertiesUpdater;
        private readonly IUserProfilePropertiesChangesNotifier userProfilePropertiesChangesNotifier;

        public ChangeUserProfilePropertiesOperationHandler(IUserProfilePropertiesChangesNotifier userProfilePropertiesChangesNotifier)
        {
            this.userProfilePropertiesChangesNotifier = userProfilePropertiesChangesNotifier;

            databaseUserProfileExistence = Server.Components.GetComponent<IDatabaseUserProfileExistence>().AssertNotNull();
            databaseUserProfilePropertiesUpdater = Server.Components.GetComponent<IDatabaseUserProfilePropertiesUpdater>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<ChangeUserProfilePropertiesRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;

            if (!databaseUserProfileExistence.Exists(userId))
            {
                LogUtils.Log($"An attempt to change properties for user id #{userId} which does not has a profile.");
                return null;
            }

            var localId = messageData.Parameters.LocalId;
            var serverType = messageData.Parameters.ServerType;
            var connectionStatus = messageData.Parameters.ConnectionStatus;
            databaseUserProfilePropertiesUpdater.Update(userId, localId, serverType, connectionStatus);
            userProfilePropertiesChangesNotifier.Notify(serverType, connectionStatus);
            return null;
        }
    }
}