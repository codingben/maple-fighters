using Characters.Service.Application;
using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace Characters.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<CharactersServiceApplication>
    {
        protected override CharactersServiceApplication CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new CharactersServiceApplication(fiberProvider, serverConnector);
        }
    }
}