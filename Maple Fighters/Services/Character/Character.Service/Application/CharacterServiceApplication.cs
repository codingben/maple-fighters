using Authorization.Server.Common;
using Character.Service.Application.PeerLogics;
using CharacterService.Application.Components;
using Database.Common.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Character.Service.Application
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

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

            WrapClientPeer(clientPeer, new InboundServerPeerLogic());
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
            AddComponents();
        }

        private void AddComponents()
        {
            Server.Components.AddComponent(new AuthorizationService());
            Server.Components.AddComponent(new DatabaseConnectionProvider());
            Server.Components.AddComponent(new DatabaseCharacterCreator());
            Server.Components.AddComponent(new DatabaseCharacterRemover());
            Server.Components.AddComponent(new DatabaseCharacterNameVerifier());
            Server.Components.AddComponent(new DatabaseCharacterGetter());
            Server.Components.AddComponent(new DatabaseCharactersGetter());
            Server.Components.AddComponent(new DatabaseCharacterExistence());
        }
    }
}