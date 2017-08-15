using Game.Application.PeerLogic;
using ServerCommunicationInterfaces;

namespace Game.Application
{
    public class GameApplication : ServerApplication.Common.ApplicationBase.Application
    {
        public override void Initialize()
        {
            AddCommonComponents();
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            AddPeerLogic(new ClientPeerLogic(clientPeer));
        }
    }
}