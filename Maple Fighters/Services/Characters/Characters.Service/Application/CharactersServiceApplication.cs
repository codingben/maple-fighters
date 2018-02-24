using Authorization.Server.Common;
using Characters.Service.Application.PeerLogics;
using CharactersService.Application.Components;
using Database.Common.Components;
using PeerLogic.Common.PeerLogic.S2S;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Characters.Service.Application
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    public class CharactersServiceApplication : ApplicationBase
    {
        public CharactersServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            var peerLogic = clientPeer.GetPeerLogic(new UnauthorizedClientPeerLogic(), new InboundServerPeerLogic());
            WrapClientPeer(clientPeer, peerLogic);
        }

        public override void Startup()
        {
            base.Startup();

            AddComponents();
        }

        private void AddComponents()
        {
            Server.Components.AddComponent(new AuthorizationService());
            Server.Components.AddComponent(new DatabaseConnectionProvider());
            Server.Components.AddComponent(new DatabaseCharacterCreator());
            Server.Components.AddComponent(new DatabaseCharacterRemover());
            Server.Components.AddComponent(new DatabaseCharacterNameVerifier());
            Server.Components.AddComponent(new DatabaseCharactersGetter());
            Server.Components.AddComponent(new DatabaseCharacterExistence());
        }
    }
}