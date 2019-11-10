using Chat.Common;
using CommonTools.Coroutines;
using Scripts.Services.Chat;
using UnityEngine;

namespace Scripts.UI.Chat
{
    [RequireComponent(typeof(IOnChatMessageReceived))]
    public class ChatInteractor : MonoBehaviour
    {
        private ChatService chatService;

        private IOnChatMessageReceived onChatMessageReceived;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            chatService = FindObjectOfType<ChatService>();

            onChatMessageReceived = GetComponent<IOnChatMessageReceived>();

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Start()
        {
            SubscribeToChatApiEvents();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();

            UnsubscribeFromChatApiEvents();
        }

        private void SubscribeToChatApiEvents()
        {
            chatService?.ChatApi.ChatMessageReceived.AddListener(OnChatMessageReceived);
        }

        private void UnsubscribeFromChatApiEvents()
        {
            chatService?.ChatApi.ChatMessageReceived.RemoveListener(OnChatMessageReceived);
        }

        public void SendChatMessage(string message)
        {
            var chatApi = chatService?.ChatApi;
            if (chatApi != null)
            {
                var parameters = new ChatMessageRequestParameters(message);

                chatApi.SendChatMessage(parameters);
            }
        }

        private void OnChatMessageReceived(ChatMessageEventParameters parameters)
        {
            var message = parameters.Message;

            onChatMessageReceived.OnMessageReceived(message);
        }
    }
}