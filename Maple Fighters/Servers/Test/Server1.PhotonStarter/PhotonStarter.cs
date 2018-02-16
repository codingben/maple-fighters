using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace Server1.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<Server1Application>
    {
        protected override Server1Application CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new Server1Application(fiberProvider, serverConnector);
        }
    }
}