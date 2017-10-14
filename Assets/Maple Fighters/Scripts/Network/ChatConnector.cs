using System.Threading.Tasks;
using Chat.Common;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.UI;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.Services
{
    public class ChatConnector : MonoSingleton<ChatConnector>
    {
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        public void Connect()
        {
            coroutinesExecutor.StartTask(Connect);
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private async Task Connect(IYield yield)
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.ChatMessageNotifier.Invoke("Connecting to a chat server...", ChatMessageColor.Green);

            var connectionStatus = await ServiceContainer.ChatService.Connect(yield);
            if (connectionStatus == ConnectionStatus.Failed)
            {
                chatWindow.ChatMessageNotifier.Invoke("Could not connect to a chat server.", ChatMessageColor.Red);
                return;
            }

            coroutinesExecutor.StartTask(Authenticate);
        }

        private async Task Authenticate(IYield yield)
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();

            var authenticationStatus = await ServiceContainer.ChatService.Authenticate(yield);
            if (authenticationStatus == AuthenticateStatus.Failed)
            {
                chatWindow.ChatMessageNotifier.Invoke("Authentication with chat server failed.", ChatMessageColor.Red);
                return;
            }

            chatWindow.ChatMessageNotifier.Invoke("Connected to a chat server successfully.", ChatMessageColor.Green);

            ChatController.Instance.OnAuthenticated();
        }
    }
}