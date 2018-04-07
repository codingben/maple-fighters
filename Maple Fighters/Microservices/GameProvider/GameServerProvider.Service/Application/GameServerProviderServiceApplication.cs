using Authorization.Client.Common;
using Authorization.Server.Common;
using GameServerProvider.Service.Application.Components;
using GameServerProvider.Service.Application.PeerLogics;
using JsonConfig;
using ServerApplication.Common.ApplicationBase;
using ServerCommunication.Common;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

namespace GameServerProvider.Service.Application
{
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

            var tcpPort = (int)Config.Global.ConnectionInfo.TcpPort;
            if (clientPeer.ConnectionInformation.Port == tcpPort)
            {
                WrapClientPeer(clientPeer, new UnauthenticatedServerPeerLogic<InboundServerPeerLogic>());
            }
            else
            {
                WrapClientPeer(clientPeer, new UnauthorizedClientPeerLogic<AuthorizedClientPeerLogic>());
            }
        }

        private void AddComponents()
        {
            ServerComponents.AddComponent(new AuthorizationService());
            ServerComponents.AddComponent(new UserProfileService());
            ServerComponents.AddComponent(new GameServersInformationStorage());
        }
    }
}