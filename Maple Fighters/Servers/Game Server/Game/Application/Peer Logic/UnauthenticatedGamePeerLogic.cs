using CommunicationHelper;
using Game.Application.PeerLogic.Operations;
using Game.InterestManagement;
using ServerCommunicationInterfaces;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;

namespace Game.Application.PeerLogic
{
    internal class UnauthenticatedGamePeerLogic : PeerLogicBase<UnauthenticatedGameOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer, int peerId)
        {
            base.Initialize(peer, peerId);

            AddHandlerForEnterWorldOperation();
        }

        private void AddHandlerForEnterWorldOperation()
        {
            OperationRequestHandlerRegister.SetHandler(UnauthenticatedGameOperations.EnterWorld, new EnterWorldOperation(OnAuthenticated));
        }

        private void OnAuthenticated(IGameObject gameObject)
        {
            PeerWrapper.SetPeerLogic(new AuthenticatedGamePeerLogic(gameObject));
        }
    }
}