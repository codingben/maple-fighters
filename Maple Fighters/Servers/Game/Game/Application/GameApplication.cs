using System.Collections.Generic;
using System.Reflection;
using Authorization.Server.Common;
using Characters.Server.Common;
using Game.Application.Components;
using Game.Application.PeerLogics;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Game.Application
{
    public class GameApplication : ApplicationBase
    {
        public GameApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
            : base(fiberProvider, serverConnector)
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

            WrapClientPeer(clientPeer, new UnauthorizedClientPeerLogic());
        }

        private void AddComponents()
        {
            CreatePythonScriptEngine();

            Server.Components.AddComponent(new CharacterSpawnDetailsProvider());
            Server.Components.AddComponent(new SceneContainer());
            Server.Components.AddComponent(new CharacterCreator());
            Server.Components.AddComponent(new AuthorizationService());
            Server.Components.AddComponent(new CharactersService());
        }

        private void CreatePythonScriptEngine()
        {
            var pythonScriptEngine = Server.Components.AddComponent(new PythonScriptEngine());
            var assemblies = GetPythonScriptEngineAssemblies();

            foreach (var assembly in assemblies)
            {
                pythonScriptEngine.GetScriptEngine().Runtime.LoadAssembly(assembly);
            }
        }

        private IEnumerable<Assembly> GetPythonScriptEngineAssemblies()
        {
            yield return typeof(MathematicsHelper.Vector2).Assembly;
            yield return typeof(InterestManagement.SceneObject).Assembly;
            yield return typeof(Physics.Box2D.PhysicsWorldInfo).Assembly;
        }
    }
}