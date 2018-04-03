using Authorization.Service.Application;
using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace Authorization.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<AuthorizationServiceApplication>
    {
        protected override AuthorizationServiceApplication CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new AuthorizationServiceApplication(fiberProvider, serverConnector);
        }
    }
}