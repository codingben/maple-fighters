using Authorization.Server.Common;
using Authorization.Service.Application.PeerLogic.Operations;
using CommunicationHelper;
using PeerLogic.Common;

namespace Authorization.Service.Application.PeerLogic
{
    internal class InboundServerPeerLogic : PeerLogicBase<AuthorizationOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper peer)
        {
            base.Initialize(peer);

            AddHandlerForRemoveAuthorizationOperation();
            AddHandlerForAccessTokenAuthorizationOperation();
            AddHandlerForUserAuthorizationOperation();
        }

        private void AddHandlerForRemoveAuthorizationOperation()
        {
            OperationHandlerRegister.SetHandler(AuthorizationOperations.RemoveAuthorization, new RemoveAuthorizationOperationHandler());
        }

        private void AddHandlerForAccessTokenAuthorizationOperation()
        {
            OperationHandlerRegister.SetHandler(AuthorizationOperations.AccessTokenAuthorization, new AccessTokenAuthorizationOperationHandler());
        }

        private void AddHandlerForUserAuthorizationOperation()
        {
            OperationHandlerRegister.SetHandler(AuthorizationOperations.UserAuthorization, new UserAuthorizationOperationHandler());
        }
    }
}