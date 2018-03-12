using Authorization.Server.Common;
using GameServerProvider.Service.Application.Components;
using GameServerProvider.Service.Application.PeerLogics;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

namespace GameServerProvider.Service.Application
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class GameServerProviderServiceApplication : ApplicationBase
    {
        public GameServerProviderServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
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

            const int TCP_PORT = 0000;
            if (clientPeer.ConnectionInformation.Port == TCP_PORT)
            {
                WrapClientPeer(clientPeer, new InboundServerPeerLogic());
            }
            else
            {
                WrapClientPeer(clientPeer, new UnauthorizedClientPeerLogic());
            }
        }

        private void AddComponents()
        {
            Server.Components.AddComponent(new AuthorizationService());
            Server.Components.AddComponent(new UserProfileService());
            Server.Components.AddComponent(new GameServersInformationStorage());
        }
    }
}