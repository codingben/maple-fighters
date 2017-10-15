using Chat.Application.PeerLogics;
using Database.Common.AccessToken;
using Database.Common.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Chat.Application
{
    public class ChatApplication : ApplicationBase
    {
        public ChatApplication(IFiberProvider fiberProvider) 
            : base(fiberProvider)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
            AddComponents();
        }

        private void AddComponents()
        {
            Server.Entity.Container.AddComponent(new DatabaseConnectionProvider());
            Server.Entity.Container.AddComponent(new DatabaseUserIdViaAccessTokenProvider());
            Server.Entity.Container.AddComponent(new DatabaseAccessTokenExistence());
            Server.Entity.Container.AddComponent(new LocalDatabaseAccessTokens());
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            WrapClientPeer(clientPeer, new UnauthenticatedPeerLogic());
        }
    }
}