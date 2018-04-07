using Authorization.Server.Common;
using Character.Service.Application.PeerLogic;
using CharacterService.Application.Components;
using Database.Common.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunication.Common;
using ServerCommunicationInterfaces;

namespace Character.Service.Application
{
    public class CharacterServiceApplication : ApplicationBase
    {
        public CharacterServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new UnauthenticatedServerPeerLogic<InboundServerPeerLogic>());
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
            AddComponents();
        }

        private void AddComponents()
        {
            ServerComponents.AddComponent(new AuthorizationService());
            ServerComponents.AddComponent(new DatabaseConnectionProvider());
            ServerComponents.AddComponent(new DatabaseCharacterCreator());
            ServerComponents.AddComponent(new DatabaseCharacterRemover());
            ServerComponents.AddComponent(new DatabaseCharacterNameVerifier());
            ServerComponents.AddComponent(new DatabaseCharacterGetter());
            ServerComponents.AddComponent(new DatabaseCharactersGetter());
            ServerComponents.AddComponent(new DatabaseCharacterExistence());
        }
    }
}