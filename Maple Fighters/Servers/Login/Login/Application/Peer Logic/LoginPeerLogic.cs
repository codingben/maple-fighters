using CommunicationHelper;
using Login.Application.PeerLogic.Operations;
using Login.Common;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

namespace Login.Application.PeerLogic
{
    internal class LoginPeerLogic : PeerLogicBase<LoginOperations, EmptyEventCode>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddComponents();

            AddHandlerForAuthenticationOperation();
        }

        private void AddComponents()
        {
            Components.AddComponent(new InactivityTimeout());
        }

        private void AddHandlerForAuthenticationOperation()
        {
            OperationHandlerRegister.SetAsyncHandler(LoginOperations.Authenticate, new AuthenticationOperationHandler(OnAuthenticated));
        }

        private void OnAuthenticated(int userId)
        {
            OperationHandlerRegister.Dispose();

            Components.AddComponent(new UserProfileTracker(userId, ServerType.Login, isUserProfileChanged: true));
        }
    }
}