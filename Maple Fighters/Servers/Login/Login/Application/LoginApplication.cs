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
        public LoginApplication(IFiberProvider fiberProvider) 
            : base(fiberProvider)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();

            Server.Entity.AddComponent(new DatabaseConnectionProvider());
            Server.Entity.AddComponent(new DatabaseUserVerifier());
            Server.Entity.AddComponent(new DatabaseUserPasswordVerifier());
            Server.Entity.AddComponent(new DatabaseUserIdProvider());
            Server.Entity.AddComponent(new DatabaseAccessTokenCreator());
            Server.Entity.AddComponent(new DatabaseAccessTokenExistence());
            Server.Entity.AddComponent(new DatabaseAccessTokenProvider());
            Server.Entity.AddComponent(new DatabaseAccessTokenExistenceViaUserId());
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new LoginPeerLogic());
        }
    }
}