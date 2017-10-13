using Chat.Common;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ChatController : MonoBehaviour
    {
        private ChatWindow chatWindow;

        private void Start()
        {
            ServiceContainer.ChatService.Authenticated += OnAuthenticated;
        }

        private void OnAuthenticated()
        {
            chatWindow = UserInterfaceContainer.Instance.Add<ChatWindow>();
            chatWindow.SendChatMessage += OnSendChatMessage;

            ServiceContainer.ChatService.ChatMessageReceived.AddListener(p => chatWindow.ChatMessageReceived.Invoke(p));
        }

        private void OnDestroy()
        {
            ServiceContainer.ChatService.Authenticated -= OnAuthenticated;

            if (chatWindow == null)
            {
                return;
            }

            chatWindow.SendChatMessage -= OnSendChatMessage;

            ServiceContainer.ChatService.ChatMessageReceived.RemoveAllListeners();

            UserInterfaceContainer.Instance.Remove(chatWindow);
        }

        private void OnSendChatMessage(string message)
        {
            var parameters = new ChatMessageRequestParameters(message);
            ServiceContainer.ChatService.SendChatMessage(parameters);
        }
    }
}