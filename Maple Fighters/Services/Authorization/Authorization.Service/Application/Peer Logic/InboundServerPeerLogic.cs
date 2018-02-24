using Authorization.Server.Common;
using Authorization.Service.Application.PeerLogic.Operations;
using CommunicationHelper;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Authorization.Service.Application.PeerLogic
{
    internal class InboundServerPeerLogic : PeerLogicBase<ServerOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForCreateAuthorizationOperation();
            AddHandlerForRemoveAuthorizationOperation();
            AddHandlerForAccessTokenAuthorizationOperation();
            AddHandlerForUserAuthorizationOperation();
        }

        private void AddHandlerForCreateAuthorizationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ServerOperations.CreateAuthorization, new CreateAuthorizationOperation());
        }

        private void AddHandlerForRemoveAuthorizationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ServerOperations.RemoveAuthorization, new RemoveAuthorizationOperation());
        }

        private void AddHandlerForAccessTokenAuthorizationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ServerOperations.AccessTokenAuthorization, new AccessTokenAuthorizationOperation());
        }

        private void AddHandlerForUserAuthorizationOperation()
        {
            OperationRequestHandlerRegister.SetHandler(ServerOperations.UserAuthorization, new UserAuthorizationOperation());
        }
    }
}