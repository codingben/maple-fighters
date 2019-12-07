using Chat.Common;
using Scripts.Services.Chat;
using UnityEngine;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(IOnChatMessageReceived))]
    public class ChatInteractor : MonoBehaviour
    {
        private ChatService chatService;
        private IOnChatMessageReceived onChatMessageReceived;

        private void Awake()
        {
            chatService = FindObjectOfType<ChatService>();
            onChatMessageReceived = GetComponent<IOnChatMessageReceived>();
        }

        private void Start()
        {
            SubscribeToChatApiEvents();
        }

        private void OnDisable()
        {
            UnsubscribeFromChatApiEvents();
        }

        private void SubscribeToChatApiEvents()
        {
            chatService?.ChatApi?.ChatMessageReceived.AddListener(OnChatMessageReceived);
        }

        private void UnsubscribeFromChatApiEvents()
        {
            chatService?.ChatApi?.ChatMessageReceived.RemoveListener(OnChatMessageReceived);
        }

        public void SendChatMessage(string message)
        {
            if (chatService != null)
            {
                var chatApi = chatService.ChatApi;
                if (chatApi != null)
                {
                    var parameters = new ChatMessageRequestParameters(message);
                    chatApi.SendChatMessage(parameters);
                }
            }
        }

        private void OnChatMessageReceived(ChatMessageEventParameters parameters)
        {
            var message = parameters.Message;
            onChatMessageReceived.OnMessageReceived(message);
        }
    }
}