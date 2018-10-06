using Authenticator.Application;
using Authenticator.Application.Peer;
using ServerCommon.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Authenticator.PhotonStarter
{
    public class AuthenticatorStartup : PhotonStarterBase<AuthenticatorApplication, AuthenticatorPeer>
    {
        protected override AuthenticatorApplication CreateApplication(
            IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new AuthenticatorApplication(fiberProvider, serverConnector);
        }
    }
}