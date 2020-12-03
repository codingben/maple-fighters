using Login.Application;
using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace Login.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<LoginApplication>
    {
        protected override LoginApplication CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new LoginApplication(fiberProvider, serverConnector);
        }
    }
}