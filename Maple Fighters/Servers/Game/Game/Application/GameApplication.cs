using Database.Common.Components;
using Game.Application.Components;
using Game.Application.PeerLogics;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Game.Application
{
    public class GameApplication : ApplicationBase
    {
        public GameApplication(IFiberProvider fiberProvider) 
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
            WrapClientPeer(clientPeer, new UnauthenticatedPeerLogic());
        }

        private void AddComponents()
        {
            Server.Entity.Container.AddComponent(new PythonScriptEngine());
            Server.Entity.Container.AddComponent(new SceneContainer());
            Server.Entity.Container.AddComponent(new PlayerGameObjectCreator());
            Server.Entity.Container.AddComponent(new DatabaseConnectionProvider());
            Server.Entity.Container.AddComponent(new DatabaseCharacterCreator());
            Server.Entity.Container.AddComponent(new DatabaseCharacterRemover());
            Server.Entity.Container.AddComponent(new DatabaseCharacterNameVerifier());
            Server.Entity.Container.AddComponent(new DatabaseCharactersGetter());
            Server.Entity.Container.AddComponent(new DatabaseCharacterExistence());
        }
    }
}