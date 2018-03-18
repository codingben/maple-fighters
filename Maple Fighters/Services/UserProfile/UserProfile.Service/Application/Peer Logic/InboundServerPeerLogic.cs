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

            AddHandlerForRegisterToUserProfileServiceOperation();
            AddHandlerForUnregisterFromUserProfileServiceOperation();
            AddHandlerForChangeUserProfilePropertiesOperation();
            AddHandlerForSubscribeToUserProfileOperation();
            AddHandlerForUnsubscribeFromUserProfileOperation();
        }
        
        private void AddHandlerForChangeUserProfilePropertiesOperation()
        {
            var userProfilePropertiesChangesNotifier = Components.AddComponent(new UserProfilePropertiesChangesNotifier());
            OperationHandlerRegister.SetHandler(UserProfileOperations.ChangeUserProfileProperties, new ChangeUserProfilePropertiesOperationHandler(userProfilePropertiesChangesNotifier));
        }

        private void AddHandlerForRegisterToUserProfileServiceOperation()
        {
            var peerId = PeerWrapper.PeerId;
            OperationHandlerRegister.SetHandler(UserProfileOperations.Register, new RegisterToUserProfileServiceOperationHandler(peerId));
        }

        private void AddHandlerForUnregisterFromUserProfileServiceOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.Unregister, new UnregisterFromUserProfileServiceOperationHandler());
        }

        private void AddHandlerForSubscribeToUserProfileOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.Subscribe, new SubscribeToUserProfileOperationHandler());
        }

        private void AddHandlerForUnsubscribeFromUserProfileOperation()
        {
            OperationHandlerRegister.SetHandler(UserProfileOperations.Unsubscribe, new UnsubscribeFromUserProfileOperationHandler());
        }
    }
}