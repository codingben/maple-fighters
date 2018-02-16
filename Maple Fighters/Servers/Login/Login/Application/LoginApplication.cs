using Database.Common.AccessToken;
using Database.Common.Components;
using Login.Application.Components;
using Login.Application.PeerLogic;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Login.Application
{
    public class LoginApplication : ApplicationBase
    {
        public LoginApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();

            Server.Components.AddComponent(new DatabaseConnectionProvider());
            Server.Components.AddComponent(new DatabaseUserVerifier());
            Server.Components.AddComponent(new DatabaseUserPasswordVerifier());
            Server.Components.AddComponent(new DatabaseUserIdProvider());
            Server.Components.AddComponent(new DatabaseAccessTokenCreator());
            Server.Components.AddComponent(new DatabaseAccessTokenExistence());
            Server.Components.AddComponent(new DatabaseAccessTokenProvider());
            Server.Components.AddComponent(new DatabaseAccessTokenExistenceViaUserId());
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new LoginPeerLogic());
        }
    }
}