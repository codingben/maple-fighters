using System.Threading.Tasks;
using Authorization.Client.Common;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using Scripts.UI.Controllers;

namespace Scripts.Services
{
    public sealed class ChatService : ServiceBase<ChatOperations, ChatEvents>, IChatService
    {
        public UnityEvent<ChatMessageEventParameters> ChatMessageReceived { get; } = new UnityEvent<ChatMessageEventParameters>();
        private AuthorizationStatus authorizationStatus = AuthorizationStatus.Failed;

        protected override void OnConnected()
        {
            base.OnConnected();

            ServerPeerHandler.SetEventHandler((byte)ChatEvents.ChatMessage, ChatMessageReceived);
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnConnected();

            ServerPeerHandler.RemoveEventHandler((byte)ChatEvents.ChatMessage);

            if (authorizationStatus == AuthorizationStatus.Failed)
            {
                ChatController.Instance.OnNonAuthorized(); // TODO: Call this from a ChatConnectionProvider
            }
        }

        public async Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            var responseParameters = await ServerPeerHandler.SendOperation<AuthorizeRequestParameters, AuthorizeResponseParameters>
                (yield, (byte)ChatOperations.Authorize, parameters, MessageSendOptions.DefaultReliable());
            authorizationStatus = responseParameters.Status;
            return responseParameters;
        }

        public void SendChatMessage(ChatMessageRequestParameters parameters)
        {
            ServerPeerHandler.SendOperation((byte)ChatOperations.ChatMessage, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}