using CommonTools.Log;
using Game.Application.PeerLogic;
using ServerCommunicationInterfaces;

namespace Game.Application
{
    public class GameApplication : ServerApplication.Common.ApplicationBase.Application
    {
        public override void Initialize()
        {
            LogUtils.Log("GameApplication::Initialize()");
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            CreateNewPeerLogic(new ClientPeerLogic(clientPeer));
        }

        protected override void Disposed()
        {
            LogUtils.Log("GameApplication::Dispose()");
        }
    }
}