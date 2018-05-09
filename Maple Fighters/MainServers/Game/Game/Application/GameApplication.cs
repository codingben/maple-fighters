using System.Collections.Generic;
using System.Reflection;
using Authorization.Client.Common;
using Authorization.Server.Common;
using Character.Server.Common;
using Game.Application.Components;
using Game.Application.Components.Python;
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
            ServerComponents.AddComponent(new AuthorizationService());
            ServerComponents.AddComponent(new CharacterService());
            ServerComponents.AddComponent(new UserProfileService());
            ServerComponents.AddComponent(new GameServerProviderService());
            ServerComponents.AddComponent(new PythonScriptEngine());
            ServerComponents.AddComponent(new PythonAssemblies());
            ServerComponents.AddComponent(new CharacterSpawnDetailsProvider());
            ServerComponents.AddComponent(new SceneContainer());
            ServerComponents.AddComponent(new PlayerGameObjectCreator());
        }
    }
}