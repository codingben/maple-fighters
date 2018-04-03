using Character.Service.Application;
using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace Character.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<CharacterServiceApplication>
    {
        protected override CharacterServiceApplication CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new CharacterServiceApplication(fiberProvider, serverConnector);
        }
    }
}