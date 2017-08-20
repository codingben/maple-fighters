using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic;
using Game.Entities;
using Game.InterestManagement;
using Game.Systems;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;

namespace Game.Application
{
    using Application = ServerApplication.Common.ApplicationBase.Application;

    public class GameApplication : Application
    {
        public override void Initialize()
        {
            AddCommonComponents();
            AddComponents();
            AddSystemsComponents();
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            var idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull() as IdGenerator;
            var peerId = idGenerator.GenerateId();

            WrapClientPeer(new ClientPeerLogic(clientPeer, peerId));
        }

        private void AddComponents()
        {
            ServerComponents.Container.AddComponent(new EntityContainer());
            ServerComponents.Container.AddComponent(new EntityIdToPeerIdConverter());
            ServerComponents.Container.AddComponent(new SceneContainer());
        }

        private void AddSystemsComponents()
        {
            ServerComponents.Container.AddComponent(new TransformSystem());
        }
    }
}