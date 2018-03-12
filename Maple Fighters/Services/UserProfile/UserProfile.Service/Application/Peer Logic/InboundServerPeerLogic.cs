using PeerLogic.Common;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components;
using UserProfile.Service.Application.PeerLogic.Operations;

namespace UserProfile.Service.Application.PeerLogic
{
    internal class InboundServerPeerLogic : PeerLogicBase<UserProfileOperations, UserProfileEvents>
    {
        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();

            AddHandlerForCreateUserProfileOperation();
            AddHandlerForChangeUserProfilePropertiesOperation();
        }

        private void AddHandlerForCreateUserProfileOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.CreateUserProfile, new CreateUserProfileOperationHandler());
        }

        private void AddHandlerForChangeUserProfilePropertiesOperation()
        {
            var userProfilePropertiesChangesNotifier = Components.AddComponent(new UserProfilePropertiesChangesNotifier());
            OperationHandlerRegister.SetHandler(UserProfileOperations.ChangeUserProfileProperties, 
                new ChangeUserProfilePropertiesOperationHandler(userProfilePropertiesChangesNotifier));
        }
    }
}