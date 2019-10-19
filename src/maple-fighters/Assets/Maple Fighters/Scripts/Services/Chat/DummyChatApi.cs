using Chat.Common;
using CommonCommunicationInterfaces;
using Network.Scripts;

namespace Scripts.Services.Chat
{
    internal class DummyChatApi : NetworkApi<ChatOperations, ChatEvents>, IChatApi
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; }

        internal DummyChatApi(IServerPeer serverPeer, bool log = false)
            : base(serverPeer, log)
        {
            ChatMessageReceived = new UnityEvent<ChatMessageEventParameters>();
        }

        public void SendChatMessage(ChatMessageRequestParameters parameters)
        {
            // Left blank intentionally
        }
    }
}