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

            Server.Components.AddComponent(new CharacterSpawnDetailsProvider());
            Server.Components.AddComponent(new SceneContainer());
            Server.Components.AddComponent(new CharacterCreator());
            Server.Components.AddComponent(new DatabaseConnectionProvider());
            Server.Components.AddComponent(new DatabaseCharacterCreator());
            Server.Components.AddComponent(new DatabaseCharacterRemover());
            Server.Components.AddComponent(new DatabaseCharacterNameVerifier());
            Server.Components.AddComponent(new DatabaseCharactersGetter());
            Server.Components.AddComponent(new DatabaseCharacterExistence());
            Server.Components.AddComponent(new DatabaseAccessTokenExistence());
            Server.Components.AddComponent(new DatabaseAccessTokenProvider());
            Server.Components.AddComponent(new DatabaseUserIdViaAccessTokenProvider());
            Server.Components.AddComponent(new LocalDatabaseAccessTokens());
        }

        private void RunPythonScriptEngine()
        {
            var pythonScriptEngine = Server.Components.AddComponent(new PythonScriptEngine());
            pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(typeof(MathematicsHelper.Vector2).Assembly);
            pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(typeof(InterestManagement.SceneObject).Assembly);
            pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(typeof(Physics.Box2D.PhysicsWorldInfo).Assembly);
        }
    }
}