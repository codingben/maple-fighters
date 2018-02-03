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

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new UnauthenticatedPeerLogic());
        }

        private void AddComponents()
        {
            Server.Components.AddComponent(new DatabaseConnectionProvider());
            Server.Components.AddComponent(new DatabaseUserIdViaAccessTokenProvider());
            Server.Components.AddComponent(new DatabaseAccessTokenExistence());
            Server.Components.AddComponent(new LocalDatabaseAccessTokens());
        }
    }
}