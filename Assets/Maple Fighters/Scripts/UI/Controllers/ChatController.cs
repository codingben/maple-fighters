using Chat.Common;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Scripts.Utils;

namespace Scripts.UI.Controllers
{
    public class ChatController : DontDestroyOnLoad<ChatController>
    {
        private void Start()
        {
            CreateChatWindow();

            ChatConnectionProvider.Instance.Connect();
        }

        private void CreateChatWindow()
        {
            var chatWindow = UserInterfaceContainer.Instance.Add<ChatWindow>();
            chatWindow.SendChatMessage += OnSendChatMessage;
            chatWindow.IsChatActive = false;
        }

        private void RemoveChatWindow()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.SendChatMessage -= OnSendChatMessage;
            UserInterfaceContainer.Instance.Remove(chatWindow);
        }

        public void OnAuthorized()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.IsChatActive = true;

            ServiceContainer.ChatService.ChatMessageReceived.AddListener(parameters => chatWindow.AddMessage(parameters.Message));
        }

        private void OnDestroy()
        {
            RemoveChatWindow();

            ServiceContainer.ChatService.ChatMessageReceived.RemoveAllListeners();
        }

        private void OnSendChatMessage(string message)
        {
            var parameters = new ChatMessageRequestParameters(message);
            ServiceContainer.ChatService.SendChatMessage(parameters);
        }
    }
}