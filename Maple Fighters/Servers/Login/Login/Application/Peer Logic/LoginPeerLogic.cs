using CommunicationHelper;
using Login.Application.PeerLogic.Operations;
using Login.Common;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace Login.Application.PeerLogic
{
    internal class LoginPeerLogic : PeerLogicBase<LoginOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddHandlerForAuthenticationOperation();
        }

        private void AddHandlerForAuthenticationOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(LoginOperations.Authenticate, new AuthenticationOperationHandler());
        }
    }
}