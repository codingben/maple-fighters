using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using JsonConfig;
using ServerCommunication.Common;
using ServerCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public class UserProfileService : ServiceBase<UserProfileOperations, UserProfileEvents>, IUserProfileServiceAPI
    {
        public event Action<UserProfilePropertiesChangedEventParameters> UserProfilePropertiesChanged;

        protected override void OnConnected(IOutboundServerPeer outboundServerPeer)
        {
            base.OnConnected(outboundServerPeer);

            OutboundServerPeerLogic?.SetEventHandler((byte)UserProfileEvents.UserProfilePropertiesChanged, UserProfilePropertiesChanged);
        }

        protected override void OnDisconnected(DisconnectReason disconnectReason, string details)
        {
            base.OnDisconnected(disconnectReason, details);

            OutboundServerPeerLogic?.RemoveEventHandler((byte)UserProfileEvents.UserProfilePropertiesChanged);
        }

        public Task<CreateUserProfileResponseParameters> CreateUserProfile(IYield yield, CreateUserProfileRequestParameters parameters)
        {
            return OutboundServerPeerLogic?.SendOperation<CreateUserProfileRequestParameters, CreateUserProfileResponseParameters>
                (yield, (byte)UserProfileOperations.CreateUserProfile, parameters);
        }

        public void ChangeUserProfileProperties(ChangeUserProfilePropertiesRequestParameters parameters)
        {
            OutboundServerPeerLogic?.SendOperation((byte)UserProfileOperations.ChangeUserProfileProperties, parameters);
        }

        protected override PeerConnectionInformation GetPeerConnectionInformation()
        {
            LogUtils.Assert(Config.Global.UserProfileService, MessageBuilder.Trace("Could not find a connection info for the User Profile service."));

            var ip = (string)Config.Global.UserProfileService.IP;
            var port = (int)Config.Global.UserProfileService.Port;
            return new PeerConnectionInformation(ip, port);
        }
    }
}