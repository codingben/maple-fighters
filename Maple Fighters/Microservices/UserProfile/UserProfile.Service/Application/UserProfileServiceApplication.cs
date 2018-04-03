using Authorization.Server.Common;
using Database.Common.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using UserProfile.Service.Application.Components;
using UserProfile.Service.Application.PeerLogic;

namespace UserProfile.Service.Application
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

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

            WrapClientPeer(clientPeer, new InboundServerPeerLogic());
        }

        private void AddComponents()
        {
            Server.Components.AddComponent(new AuthorizationService());
            Server.Components.AddComponent(new DatabaseConnectionProvider());
            Server.Components.AddComponent(new DatabaseUserProfileCreator());
            Server.Components.AddComponent(new DatabaseUserProfilePropertiesUpdater());
            Server.Components.AddComponent(new DatabaseUserProfileExistence());
            Server.Components.AddComponent(new ServerIdToPeerIdConverter());
            Server.Components.AddComponent(new UserIdToServerIdConverter());
        }
    }
}