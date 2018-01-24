using Database.Common.AccessToken;
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
            base.OnConnected(clientPeer);

            WrapClientPeer(clientPeer, new UnauthenticatedPeerLogic());
        }

        private void AddComponents()
        {
            RunPythonScriptEngine();

            Server.Entity.AddComponent(new CharacterSpawnDetailsProvider());
            Server.Entity.AddComponent(new SceneContainer());
            Server.Entity.AddComponent(new CharacterCreator());
            Server.Entity.AddComponent(new DatabaseConnectionProvider());
            Server.Entity.AddComponent(new DatabaseCharacterCreator());
            Server.Entity.AddComponent(new DatabaseCharacterRemover());
            Server.Entity.AddComponent(new DatabaseCharacterNameVerifier());
            Server.Entity.AddComponent(new DatabaseCharactersGetter());
            Server.Entity.AddComponent(new DatabaseCharacterExistence());
            Server.Entity.AddComponent(new DatabaseAccessTokenExistence());
            Server.Entity.AddComponent(new DatabaseAccessTokenProvider());
            Server.Entity.AddComponent(new DatabaseUserIdViaAccessTokenProvider());
            Server.Entity.AddComponent(new LocalDatabaseAccessTokens());
        }

        private void RunPythonScriptEngine()
        {
            var pythonScriptEngine = Server.Entity.AddComponent(new PythonScriptEngine());
            pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(typeof(MathematicsHelper.Vector2).Assembly);
            pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(typeof(InterestManagement.SceneObject).Assembly);
            pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(typeof(Physics.Box2D.PhysicsWorldInfo).Assembly);
        }
    }
}