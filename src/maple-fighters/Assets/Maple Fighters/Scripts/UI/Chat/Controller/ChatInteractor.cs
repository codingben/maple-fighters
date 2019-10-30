using Chat.Common;
using CommonTools.Coroutines;
using Scripts.Services.Chat;
using UnityEngine;

namespace Scripts.UI.Chat
{
    public class ChatInteractor : MonoBehaviour
    {
        private IChatService chatService;

        private ExternalCoroutinesExecutor coroutinesExecutor;

        private void Awake()
        {
            chatService = ChatService.GetInstance();

            coroutinesExecutor = new ExternalCoroutinesExecutor();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnDestroy()
        {
            coroutinesExecutor?.Dispose();
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
    }
}