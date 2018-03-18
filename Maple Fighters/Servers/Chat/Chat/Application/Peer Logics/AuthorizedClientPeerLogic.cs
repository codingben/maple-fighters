using Chat.Application.PeerLogic.Components;
using Chat.Application.PeerLogic.Operations;
using Chat.Common;
using CommonTools.Log;
using PeerLogic.Common;
using ServerCommunicationInterfaces;
using UserProfile.Server.Common;

namespace Chat.Application.PeerLogics
{
    internal class AuthorizedClientPeerLogic : PeerLogicBase<ChatOperations, ChatEvents>
    {
        private readonly int userId;

        public AuthorizedClientPeerLogic(int userId)
        {
            this.userId = userId;
        }

        public override void Initialize(IClientPeerWrapper<IClientPeer> peer)
        {
            base.Initialize(peer);

            AddCommonComponents();
            AddComponents();

            AddHandlerForChatMessageOperation();
        }

        private void AddComponents()
        {
            Components.AddComponent(new ChatMessageEventSender());
            Components.AddComponent(new UserProfileTracker(userId, ServerType.Game));
        }

        private void AddHandlerForChatMessageOperation()
        {
            var eventSenderWrapper = Components.GetComponent<IChatMessageEventSender>().AssertNotNull();
            OperationHandlerRegister.SetHandler(ChatOperations.ChatMessage, new ChatMessageOperationHandler(eventSenderWrapper));
        }
    }
}