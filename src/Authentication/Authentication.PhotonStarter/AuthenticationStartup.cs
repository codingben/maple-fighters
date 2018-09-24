using Authentication.Application;
using ServerCommon.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Authentication.PhotonStarter
{
    public class AuthenticationStartup : PhotonStarterBase<AuthenticationApplication>
    {
        protected override AuthenticationApplication CreateApplication(
            IFiberProvider fiberProvider,
            IServerConnector serverConnector)
        {
            return new AuthenticationApplication(
                fiberProvider,
                serverConnector);
        }
    }
}