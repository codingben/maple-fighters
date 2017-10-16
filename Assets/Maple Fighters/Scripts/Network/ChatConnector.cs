using System.Threading.Tasks;
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
        protected override void OnAwake()
        {
            base.OnAwake();

            DontDestroyOnLoad();
        }

        public void Connect()
        {
            CoroutinesExecutor.StartTask(Connect);
        }

        private async Task Connect(IYield yield)
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.ChatMessageNotifier.Invoke("Connecting to a chat server...", ChatMessageColor.Green);

            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Chat);
            var connectionStatus = await Connect(yield, ServiceContainer.ChatService, connectionInformation);
            if (connectionStatus == ConnectionStatus.Failed)
            {
                chatWindow.ChatMessageNotifier.Invoke("Could not connect to a chat server.", ChatMessageColor.Red);
                return;
            }

            CoroutinesExecutor.StartTask(Authenticate);
        }

        private async Task Authenticate(IYield yield)
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();

            var authenticationStatus = await ServiceContainer.ChatService.Authenticate(yield);
            if (authenticationStatus == AuthenticationStatus.Failed)
            {
                ServiceContainer.ChatService.Disconnect();

                chatWindow.ChatMessageNotifier.Invoke("Authentication with chat server failed.", ChatMessageColor.Red);
                return;
            }

            chatWindow.ChatMessageNotifier.Invoke("Connected to a chat server successfully.", ChatMessageColor.Green);

            ChatController.Instance.OnAuthenticated();
        }
    }
}