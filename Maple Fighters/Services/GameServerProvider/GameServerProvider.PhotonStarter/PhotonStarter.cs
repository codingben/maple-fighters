using GameServerProvider.Service.Application;
using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace GameServerProvider.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<GameServerProviderServiceApplication>
    {
        protected override GameServerProviderServiceApplication CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new GameServerProviderServiceApplication(fiberProvider, serverConnector);
        }
    }
}