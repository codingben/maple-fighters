using Game.Application.PeerLogic;
using Game.Entities;
using ServerApplication.Common.ComponentModel;
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
            WrapClientPeer(new ClientPeerLogic(clientPeer));
        }
    }
}