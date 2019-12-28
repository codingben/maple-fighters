using Authenticator.Application;
using Authenticator.Application.Peer;
using ServerCommon.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Authenticator.PhotonStarter
{
    public class AuthenticatorStartup : PhotonStarterBase<AuthenticatorApplication, AuthenticatorPeer>
    {
        protected override AuthenticatorApplication CreateApplication(
            IServerConnector serverConnector,
            IFiberProvider fiberProvider)
        {
            return new AuthenticatorApplication(serverConnector, fiberProvider);
        }
    }
}