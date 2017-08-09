using System.Collections.Generic;
using CommonTools.Log;
using Game.Application.PeerLogic;
using ServerCommunicationInterfaces;

namespace Game.Application
{
    public class GameApplication : ServerApplication.Common.ApplicationBase.Application
    {
        private readonly List<ClientPeerLogic> peerLogics = new List<ClientPeerLogic>();

        public override void Initialize()
        {
            LogUtils.Log("GameApplication::Initialize()");
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            peerLogics.Add(new ClientPeerLogic(clientPeer));
        }

        public override void Dispose()
        {
            LogUtils.Log("GameApplication::Dispose()");

            foreach (var peer in peerLogics)
            {
                peer.Dispose();
            }

            peerLogics.Clear();
        }
    }
}