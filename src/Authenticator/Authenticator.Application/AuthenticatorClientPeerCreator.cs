using Authenticator.Application.Peer.Logic;
using Common.Components;
using ServerCommon.Application;
using ServerCommon.Application.Components;
using ServerCommunicationInterfaces;

namespace Authenticator.Application
{
    public class AuthenticatorClientPeerCreator : IClientPeerCreator
    {
        private readonly IClientPeerContainer clientPeerContainer;
        private readonly IIdGenerator idGenerator;

        public AuthenticatorClientPeerCreator()
        {
            clientPeerContainer = ServerExposedComponents
                .Provide()
                .Get<IClientPeerContainer>();
            idGenerator = ServerExposedComponents
                .Provide()
                .Get<IIdGenerator>();
        }

        public void Create(IClientPeer peer)
        {
            var id = idGenerator.GenerateId();
            var peerLogic = new AuthenticatorClientPeer(peer);

            clientPeerContainer.Add(id, new ClientPeerWrapper(id, peer, peerLogic));
        }
    }
}