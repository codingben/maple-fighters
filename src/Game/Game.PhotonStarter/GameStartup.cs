using System;
using Game.Application;
using ServerCommon.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Game.PhotonStarter
{
    public class GameStartup : PhotonStarterBase<GameApplication>
    {
        protected override GameApplication CreateApplication(IServerConnector serverConnector, IFiberProvider fiberProvider)
        {
            return new GameApplication(serverConnector, fiberProvider);
        }

        protected override void CreateClientPeer(IClientPeer peer)
        {
            throw new NotImplementedException();
        }
    }
}