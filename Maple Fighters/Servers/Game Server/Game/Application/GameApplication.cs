using CommonTools.Log;
using Game.Application.PeerLogic;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;
using Shared.Game.Common;

namespace Game.Application
{
    using Application = ServerApplication.Common.ApplicationBase.Application;

    public class GameApplication : Application
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

            CreateScenes();
        }
        
        public override void OnConnected(IClientPeer clientPeer)
        {
            WrapClientPeer(clientPeer, new UnauthenticatedGamePeerLogic());
        }

        private void AddComponents()
        {
            Server.Entity.Container.AddComponent(new SceneContainer());
        }

        private void CreateScenes()
        {
            var sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
            sceneContainer.AddScene(Maps.Map_1, new Vector2(40, 10), new Vector2(10, 5));
            sceneContainer.AddScene(Maps.Map_2, new Vector2(30, 30), new Vector2(15, 10));
        }
    }
}