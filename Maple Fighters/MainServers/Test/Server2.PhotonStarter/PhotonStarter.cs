using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace Server2.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<Server2Application>
    {
        protected override Server2Application CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new Server2Application(fiberProvider, serverConnector);
        }
    }
}