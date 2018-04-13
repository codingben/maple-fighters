using System.Threading.Tasks;
using Authorization.Client.Common;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.UI;
using Scripts.UI.Controllers;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.Services
{
    public class ChatConnectionProvider : ServiceConnectionProviderBase<ChatConnectionProvider>
    {
        private AuthorizationStatus authorizationStatus = AuthorizationStatus.Failed;

        public void Connect()
        {
            var serverConnectionInformation = GetServerConnectionInformation(ServerType.Chat);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, serverConnectionInformation));
        }

        protected override void OnPreConnection()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.AddMessage("Connecting to a chat server...", ChatMessageColor.Green);
        }

        protected override void OnConnectionFailed()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.AddMessage("Could not connect to a chat server.", ChatMessageColor.Red);
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask(Authorize);
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnDisconnected(reason, details);

            if (authorizationStatus == AuthorizationStatus.Succeed)
            {
                ChatController.Instance?.OnConnectionClosed();
            }
        }

        protected override Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            var authorizationPeerLogic = GetServiceBase().GetPeerLogic<IAuthorizationPeerLogicAPI>().AssertNotNull();
            return authorizationPeerLogic.Authorize(yield, parameters);
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            ChatController.Instance?.OnNonAuthorized();
        }

        protected override void OnAuthorized()
        {
            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.AddMessage("Connected to a chat server successfully.", ChatMessageColor.Green);

            authorizationStatus = AuthorizationStatus.Succeed;

            ChatController.Instance.OnAuthorized();
        }

        protected override void SetPeerLogicAfterAuthorization()
        {
            GetServiceBase().SetPeerLogic<ChatPeerLogic, ChatOperations, ChatEvents>();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.ChatService;
        }
    }
}