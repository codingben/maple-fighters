using System;
using Photon.SocketServer;

namespace Game.Application
{
    internal class GameApplication : ApplicationBase
    {
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new PeerLogic.PeerLogic(initRequest);
        }

        protected override void Setup()
        {
            throw new NotImplementedException();
        }

        protected override void TearDown()
        {
            throw new NotImplementedException();
        }
    }
}