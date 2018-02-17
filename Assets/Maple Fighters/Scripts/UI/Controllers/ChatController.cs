using Chat.Common;
using CommonCommunicationInterfaces;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.UI.Controllers
{
    public class ChatController : DontDestroyOnLoad<ChatController>
    {
        private ChatWindow chatWindow;

        private void Start()
        {
            chatWindow = UserInterfaceContainer.Instance.Add<ChatWindow>();
            chatWindow.IsConnected = false;

            ChatConnector.Instance.Connect();
        }

        public void OnAuthenticated()
        {
            chatWindow.SendChatMessage += OnSendChatMessage;
            chatWindow.IsConnected = true;

            ServiceContainer.ChatService.ChatMessageReceived.AddListener(p => chatWindow.ChatMessageReceived.Invoke(p));
        }

        private void OnDestroy()
        {
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
            ServiceContainer.ChatService.SendOperation((byte)ChatOperations.ChatMessage, parameters, MessageSendOptions.DefaultReliable());
        }
    }
}