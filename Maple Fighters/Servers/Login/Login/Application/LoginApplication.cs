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

            Server.Entity.Container.AddComponent(new DatabaseConnectionProvider());
            Server.Entity.Container.AddComponent(new DatabaseUserVerifier());
            Server.Entity.Container.AddComponent(new DatabaseUserPasswordVerifier());
            Server.Entity.Container.AddComponent(new DatabaseUserIdProvider());
            Server.Entity.Container.AddComponent(new DatabaseAccessTokenCreator());
            Server.Entity.Container.AddComponent(new DatabaseAccessTokenExistence());
            Server.Entity.Container.AddComponent(new DatabaseAccessTokenProvider());
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            WrapClientPeer(clientPeer, new LoginPeerLogic());
        }
    }
}