using CommonTools.Log;
using Game.Application.PeerLogic;
using Game.Entities;
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

            ServerComponents.Container.AddComponent(new EntityContainer());
        }

        public override void OnConnected(IClientPeer clientPeer)
        {
            var idGenerator = ServerComponents.Container.GetComponent<IdGenerator>().AssertNotNull() as IdGenerator;
            var peerId = idGenerator.GenerateId();

            WrapClientPeer(new ClientPeerLogic(clientPeer, peerId), peerId);
        }
    }
}