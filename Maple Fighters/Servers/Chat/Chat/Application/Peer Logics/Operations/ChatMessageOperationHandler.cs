using Chat.Application.PeerLogic.Components;
using Chat.Common;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Chat.Application.PeerLogic.Operations
{
    internal class ChatMessageOperationHandler : IOperationRequestHandler<ChatMessageRequestParameters, EmptyParameters>
    {
        private readonly IChatMessageEventSender eventSenderWrapper;

        public ChatMessageOperationHandler(IChatMessageEventSender eventSenderWrapper)
        {
            this.eventSenderWrapper = eventSenderWrapper;
        }

        public EmptyParameters? Handle(MessageData<ChatMessageRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var message = messageData.Parameters.Message;
            eventSenderWrapper.SendChatMessage(message);
            return null;
        }
    }
}