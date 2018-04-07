using Authorization.Server.Common;
using Database.Common.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunication.Common;
using ServerCommunicationInterfaces;
using UserProfile.Service.Application.Components;
using UserProfile.Service.Application.PeerLogic;

namespace UserProfile.Service.Application
{
    public class UserProfileServiceApplication : ApplicationBase
    {
        public UserProfileServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
            AddComponents();
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new UnauthenticatedServerPeerLogic<InboundServerPeerLogic>());
        }

        private void AddComponents()
        {
            ServerComponents.AddComponent(new AuthorizationService());
            ServerComponents.AddComponent(new DatabaseConnectionProvider());
            ServerComponents.AddComponent(new DatabaseUserProfileCreator());
            ServerComponents.AddComponent(new DatabaseUserProfilePropertiesUpdater());
            ServerComponents.AddComponent(new DatabaseUserProfileExistence());
            ServerComponents.AddComponent(new ServerIdToPeerIdConverter());
            ServerComponents.AddComponent(new UserIdToServerIdConverter());
        }
    }
}