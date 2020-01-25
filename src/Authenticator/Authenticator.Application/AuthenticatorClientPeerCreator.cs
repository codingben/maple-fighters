using Authenticator.Application.Peer.Logic;
using Common.Components;
using ServerCommon.Application;
using ServerCommon.Application.Components;
using ServerCommunicationInterfaces;

namespace Authenticator.Application
{
    public class AuthenticatorClientPeerCreator : IClientPeerCreator
    {
        private readonly IIdGenerator idGenerator;
        private readonly IClientPeerContainer clientPeerContainer;

        public AuthenticatorClientPeerCreator()
        {
            idGenerator = ServerExposedComponents
                .Provide()
                .Get<IIdGenerator>();
            clientPeerContainer = ServerExposedComponents
                .Provide()
                .Get<IClientPeerContainer>();
        }

        public void Create(IClientPeer peer)
        {
            var id = idGenerator.GenerateId();
            var peerLogic = new AuthenticatorClientPeer(peer);
            var clientPeerWrapper = 
                new ClientPeerWrapper(id, peer, peerLogic, clientPeerContainer);
            
            clientPeerContainer.Add(id, clientPeerWrapper);
        }
    }
}