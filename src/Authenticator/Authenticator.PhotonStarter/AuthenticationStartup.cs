using Authenticator.Application;
using ServerCommon.Application;
using ServerCommon.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Authenticator.PhotonStarter
{
    public class AuthenticatorStartup : PhotonStarterBase<AuthenticatorApplication>
    {
        private readonly IClientPeerCreator clientPeerCreator;

        public AuthenticatorStartup()
        {
            clientPeerCreator = new AuthenticatorClientPeerCreator();
        }

        protected override AuthenticatorApplication CreateApplication(IServerConnector serverConnector, IFiberProvider fiberProvider)
        {
            return new AuthenticatorApplication(serverConnector, fiberProvider);
        }

        protected override void CreateClientPeer(IClientPeer clientPeer)
        {
            clientPeerCreator.Create(clientPeer);
        }
    }
}