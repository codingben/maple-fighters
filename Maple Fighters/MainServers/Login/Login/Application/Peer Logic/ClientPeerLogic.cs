using CommunicationHelper;
using Login.Application.PeerLogic.Operations;
using Login.Common;
using PeerLogic.Common;
using PeerLogic.Common.Components;
using UserProfile.Server.Common;

namespace Login.Application.PeerLogic
{
    internal class ClientPeerLogic : PeerLogicBase<LoginOperations, EmptyEventCode>
    {
        protected override void OnInitialized()
        {
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