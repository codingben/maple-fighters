using CommunicationHelper;
using Login.Application.PeerLogic.Operations;
using Login.Common;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

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
            OperationHandlerRegister.SetAsyncHandler(LoginOperations.Authenticate, new AuthenticationOperationHandler(OnAuthenticated));
        }

        private void OnAuthenticated(int userId)
        {
            OperationHandlerRegister.Dispose();

            AddCommonComponents();

            Components.AddComponent(new UserProfileTracker(userId, ServerType.Login, isUserProfileChanged: true));
        }
    }
}