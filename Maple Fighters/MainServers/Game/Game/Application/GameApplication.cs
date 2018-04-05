using System.Collections.Generic;
using System.Reflection;
using Authorization.Client.Common;
using Authorization.Server.Common;
using Character.Server.Common;
using Game.Application.Components;
using Game.Application.PeerLogics;
using GameServerProvider.Server.Common;
using PythonScripting;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

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

            WrapClientPeer(clientPeer, new UnauthorizedClientPeerLogic<CharacterSelectionPeerLogic>());
        }

        private void AddComponents()
        {
            CreatePythonScriptEngine();

            Server.Components.AddComponent(new AuthorizationService());
            Server.Components.AddComponent(new CharacterService());
            Server.Components.AddComponent(new UserProfileService());
            Server.Components.AddComponent(new GameServerProviderService());
            Server.Components.AddComponent(new CharacterSpawnDetailsProvider());
            Server.Components.AddComponent(new SceneContainer());
            Server.Components.AddComponent(new CharacterCreator());
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

        /// <summary>
        /// Getting assemblies which can not be found by python script engine
        /// </summary>
        private IEnumerable<Assembly> GetPythonScriptEngineAssemblies()
        {
            yield return typeof(MathematicsHelper.Vector2).Assembly;
            yield return typeof(InterestManagement.Components.SceneObject).Assembly;
            yield return typeof(Physics.Box2D.Core.PhysicsWorldInfo).Assembly;
        }
    }
}