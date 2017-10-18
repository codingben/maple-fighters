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

            AddHandlerForLoginOperation();
        }

        private void AddHandlerForLoginOperation()
        {
            OperationRequestHandlerRegister.SetHandler(LoginOperations.Login, new LoginOperationHandler());
        }
    }
}