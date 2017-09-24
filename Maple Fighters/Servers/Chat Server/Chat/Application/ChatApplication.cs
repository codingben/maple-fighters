using Chat.Application.PeerLogic;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Chat.Application
{
    public class ChatApplication : ApplicationBase
    {
        public ChatApplication(IFiberProvider fiberProvider) 
            : base(fiberProvider)
        {
            // Left blank intentionally
        }
        
        public override void OnConnected(IClientPeer clientPeer)
        {
            WrapClientPeer(clientPeer, new ChatPeerLogic());
        }
    }
}