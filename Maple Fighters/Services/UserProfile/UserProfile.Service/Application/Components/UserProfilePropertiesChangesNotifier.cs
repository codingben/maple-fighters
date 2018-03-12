using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common.Components;
using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components
{
    internal class UserProfilePropertiesChangesNotifier : Component, IUserProfilePropertiesChangesNotifier
    {
        private IEventSenderWrapper eventSenderWrapper;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSenderWrapper = Components.GetComponent<IEventSenderWrapper>().AssertNotNull();
        }


        public void Notify(ServerType serverType, ConnectionStatus connectionStatus)
        {
            var parameters = new UserProfilePropertiesChangedEventParameters(serverType, connectionStatus);
            eventSenderWrapper.Send((byte)UserProfileEvents.UserProfilePropertiesChanged, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}