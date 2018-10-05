using Authenticator.Application.Peer.Logic;
using CommonCommunicationInterfaces;
using ServerCommon.PeerBase;

namespace Authenticator.Application.Peer
{
    public class AuthenticatorPeer : InboundPeerBase
    {
        protected override void OnConnected()
        {
            base.OnConnected();

            BindPeerLogic<MainPeerLogic>();
        }

        protected override void OnDisconnected(
            DisconnectReason reason,
            string details)
        {
            base.OnDisconnected(reason, details);

            UnbindPeerLogic();
        }
    }
}