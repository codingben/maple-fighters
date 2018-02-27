using Chat.Common;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.ScriptableObjects;
using Scripts.UI;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.Services
{
    public class ChatConnector : ServiceConnector<ChatConnector>
    {
        public void Connect()
        {
            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Chat);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, ServiceContainer.ChatService, connectionInformation));
        }

        protected override void OnPreConnection()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.ChatMessageNotifier.Invoke("Connecting to a chat server...", ChatMessageColor.Green);
        }

        protected override void OnConnectionFailed()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.ChatMessageNotifier.Invoke("Could not connect to a chat server.", ChatMessageColor.Red);
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask((yield) => Authorize(yield, (byte)ChatOperations.Authorize));
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.ChatMessageNotifier.Invoke("Authentication with chat server failed.", ChatMessageColor.Red);

            ServiceContainer.ChatService.Dispose();
        }

        protected override void OnAuthorized()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.ChatMessageNotifier.Invoke("Connected to a chat server successfully.", ChatMessageColor.Green);

            ChatController.Instance.OnAuthorized();
        }
    }
}