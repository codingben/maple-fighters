using Authentication.Application.PeerLogic;
using CommonTools.Log;
using ServerCommon.Application;
using ServerCommunicationInterfaces;

namespace Authentication.Application
{
    public class AuthenticationApplication : ServerApplicationBase
    {
        public AuthenticationApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        protected override void OnStartup()
        {
            base.OnStartup();

            LogUtils.Log("OnStartup");

            AddS2SRelatedComponents();
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            LogUtils.Log("OnShutdown");
        }

        protected override void OnConnected(IClientPeer clientPeer, int peerId)
        {
            base.OnConnected(clientPeer, peerId);

            WrapClientPeer<ClientPeerLogic>(clientPeer, peerId);
        }
    }
}