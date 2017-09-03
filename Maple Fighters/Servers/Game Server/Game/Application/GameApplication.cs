using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic;
using Game.Entities;
using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
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
            base.Startup();

            AddCommonComponents();
            AddComponents();

            SetupScenes();
        }

        public override void Shutdown()
        {
            base.Shutdown();

            ServerComponents.Container.RemoveAllComponents();
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            var idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull();
            var peerId = idGenerator.GenerateId();

            WrapClientPeer(new GamePeerLogic(clientPeer, peerId));
        }

        private void AddComponents()
        {
            ServerComponents.Container.AddComponent(new EntityContainer());
            ServerComponents.Container.AddComponent(new EntityIdToPeerIdConverter());
            ServerComponents.Container.AddComponent(new SceneContainer());
        }

        private void SetupScenes()
        {
            var sceneContainer = ServerComponents.Container.GetComponent<SceneContainer>().AssertNotNull();
            sceneContainer.AddScene(1, new Vector2(20, 15), new Vector2(10, 5));
        }
    }
}