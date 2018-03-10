using System.Threading.Tasks;
using Authorization.Client.Common;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;

namespace Scripts.Services
{
    public sealed class ChatService : ServiceBase<ChatOperations, ChatEvents>, IChatServiceAPI
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; } = new UnityEvent<ChatMessageEventParameters>();

        protected override void OnConnected()
        {
            base.OnConnected();

            ServerPeerHandler.SetEventHandler((byte)ChatEvents.ChatMessage, ChatMessageReceived);
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnDisconnected(reason, details);

            ServerPeerHandler.RemoveEventHandler((byte)ChatEvents.ChatMessage);
        }

        public async Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            return await ServerPeerHandler.SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>
                (yield, (byte)ChatOperations.Authorize, parameters, MessageSendOptions.DefaultReliable());
        }

        public void SendChatMessage(ChatMessageRequestParameters parameters)
        {
            ServerPeerHandler.SendOperation((byte)ChatOperations.ChatMessage, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}