using Scripts.Services.Chat;
using UnityEngine;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(IOnChatMessageReceived))]
    public class ChatInteractor : MonoBehaviour
    {
        private IChatApi chatApi;
        private IOnChatMessageReceived onChatMessageReceived;

        private void Awake()
        {
            chatApi = FindObjectOfType<ChatApi>();
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
            if (chatApi != null)
            {
                chatApi.ChatMessageReceived += OnChatMessageReceived;
            }
        }

        private void UnsubscribeFromChatApiEvents()
        {
            if (chatApi != null)
            {
                chatApi.ChatMessageReceived -= OnChatMessageReceived;
            }
        }

        public void SendChatMessage(string message)
        {
            chatApi?.SendChatMessage(message);
        }

        private void OnChatMessageReceived(string message)
        {
            onChatMessageReceived.OnMessageReceived(message);
        }
    }
}