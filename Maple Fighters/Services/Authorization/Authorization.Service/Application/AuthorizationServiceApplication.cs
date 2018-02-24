using Authorization.Service.Application.PeerLogic;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Authorization.Service.Application
{
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

            WrapClientPeer(clientPeer, new InboundServerPeerLogic());
        }

        public override void Startup()
        {
            base.Startup();

        }

        private void AddComponents()
        {

        }
    }
}