using Characters.Service.Application.PeerLogics;
using CharactersService.Application.Components;
using CommonTools.Log;
using Database.Common.AccessToken;
using Database.Common.Components;
using JsonConfig;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class CharactersServiceApplication : ApplicationBase
    {
        public CharactersServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            LogUtils.Assert(Config.Global.ConnectionInfo, MessageBuilder.Trace("Could not find an connection info for the server."));

            var udpPort = (int)Config.Global.ConnectionInfo.UdpPort;
            var tcpPort = (int)Config.Global.ConnectionInfo.TcpPort;

            var clientConnectionInfo = clientPeer.ConnectionInformation;
            if (clientConnectionInfo.Port == udpPort)
            {
                WrapClientPeer(clientPeer, new UnauthenticatedClientPeerLogic());
            }
            else if (clientConnectionInfo.Port == tcpPort)
            {
                WrapClientPeer(clientPeer, new AuthenticatedServerPeerLogic());
            }
            else
            {
                LogUtils.Log($"No handler found for peer: {clientConnectionInfo.Ip}:{clientConnectionInfo.Port}");
            }
        }

        public override void Startup()
        {
            base.Startup();

            AddComponents();
        }

        private void AddComponents()
        {
            Server.Components.AddComponent(new DatabaseConnectionProvider());
            Server.Components.AddComponent(new DatabaseAccessTokenExistence());
            Server.Components.AddComponent(new DatabaseAccessTokenProvider());
            Server.Components.AddComponent(new DatabaseUserIdViaAccessTokenProvider());
            Server.Components.AddComponent(new LocalDatabaseAccessTokens());
            Server.Components.AddComponent(new DatabaseCharacterCreator());
            Server.Components.AddComponent(new DatabaseCharacterRemover());
            Server.Components.AddComponent(new DatabaseCharacterNameVerifier());
            Server.Components.AddComponent(new DatabaseCharactersGetter());
            Server.Components.AddComponent(new DatabaseCharacterExistence());
        }
    }
}