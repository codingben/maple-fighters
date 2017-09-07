using CommonTools.Log;
using Game.Application.PeerLogic;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;

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
            LogUtils.Log(MessageBuilder.Trace(string.Empty));

            base.Startup();

            AddCommonComponents();
            AddComponents();

            CreateScenes();
        }

        public override void Shutdown()
        {
            LogUtils.Log(MessageBuilder.Trace(string.Empty));

            base.Shutdown();

            ServerComponents.Container.RemoveAllComponents();
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            WrapClientPeer(clientPeer, new UnauthenticatedGamePeerLogic());
        }

        private void AddComponents()
        {
            ServerComponents.Container.AddComponent(new SceneContainer());
        }

        private void CreateScenes()
        {
            var sceneContainer = ServerComponents.Container.GetComponent<SceneContainer>().AssertNotNull();
            sceneContainer.AddScene(1, new Vector2(20, 15), new Vector2(10, 5));
        }
    }
}