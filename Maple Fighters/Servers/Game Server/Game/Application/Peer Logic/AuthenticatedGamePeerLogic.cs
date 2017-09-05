using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;
using Shared.Game.Common;

namespace Game.Application.PeerLogic
{
    internal class AuthenticatedGamePeerLogic : PeerLogicBase<AuthenticatedGameOperations, GameEvents>
    {
        private readonly IGameObject playerGameObject;

        public AuthenticatedGamePeerLogic(IGameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer, int peerId)
        {
            base.Initialize(peer, peerId);

            PeerWrapper.Disconnected += OnDisconnected;

            AddCommonComponents();

            AddHandlerForUpdateEntityPosition();
        }

        private void AddHandlerForUpdateEntityPosition()
        {
            var transform = playerGameObject.Entity.GetComponent<Transform>().AssertNotNull();
            OperationRequestHandlerRegister.SetHandler(AuthenticatedGameOperations.UpdateEntityPosition, new UpdateEntityPositionOperation(transform));
        }

        private void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            playerGameObject.Dispose();

            PeerWrapper.Disconnected -= OnDisconnected;
        }
    }
}