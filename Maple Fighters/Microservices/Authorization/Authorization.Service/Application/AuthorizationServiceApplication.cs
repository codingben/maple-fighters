using Authorization.Service.Application.Components;
using Authorization.Service.Application.PeerLogic;
using ServerApplication.Common.ApplicationBase;
using ServerCommunication.Common;
using ServerCommunicationInterfaces;

namespace Authorization.Service.Application
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class AuthorizationServiceApplication : ApplicationBase
    {
        public AuthorizationServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new UnauthenticatedServerPeerLogic<InboundServerPeerLogic>());
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
            AddComponents();
        }

        private void AddComponents()
        {
            Server.Components.AddComponent(new AccessTokensStorage());
        }
    }
}