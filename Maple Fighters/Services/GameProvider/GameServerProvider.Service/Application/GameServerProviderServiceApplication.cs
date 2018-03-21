using Authorization.Server.Common;
using GameServerProvider.Service.Application.Components;
using GameServerProvider.Service.Application.PeerLogics;
using JsonConfig;
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

            var tcpPort = (int)Config.Global.ConnectionInfo.TcpPort;
            if (clientPeer.ConnectionInformation.Port == tcpPort)
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