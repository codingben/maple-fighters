using Authenticator.Application;
using ServerCommon.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Authenticator.PhotonStarter
{
    public class AuthenticatorStartup : PhotonStarterBase<AuthenticatorApplication>
    {
        protected override AuthenticatorApplication CreateApplication(IServerConnector serverConnector, IFiberProvider fiberProvider)
        {
            return new AuthenticatorApplication(serverConnector, fiberProvider);
        }

        protected override void CreateClientPeer(IClientPeer clientPeer)
        {
            throw new System.NotImplementedException();
        }
    }
}