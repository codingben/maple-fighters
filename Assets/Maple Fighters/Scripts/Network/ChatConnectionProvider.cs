using System.Threading.Tasks;
using Authorization.Client.Common;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using CommunicationHelper;
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
            ServiceContainer.ChatService.SetPeerLogic<AuthorizationService, AuthorizationOperations, EmptyEventCode>(new AuthorizationService());

            CoroutinesExecutor.StartTask(Authorize);
        }

        protected override void OnDisconnected(DisconnectReason reason, string details)
        {
            base.OnDisconnected(reason, details);

            if (authorizationStatus == AuthorizationStatus.Failed)
            {
                ChatController.Instance.OnNonAuthorized();
            }
        }

        protected override Task<AuthorizeResponseParameters> Authorize(IYield yield, AuthorizeRequestParameters parameters)
        {
            var authorizationService = ServiceContainer.ChatService.GetPeerLogic<IAuthorizationServiceAPI>().AssertNotNull();
            return authorizationService.Authorize(yield, parameters);
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnAuthorized()
        {
            ServiceContainer.ChatService.SetPeerLogic<ChatService, ChatOperations, ChatEvents>(new ChatService());

            var chatWindow = UserInterfaceContainer.Instance.Get<ChatWindow>().AssertNotNull();
            chatWindow.AddMessage("Connected to a chat server successfully.", ChatMessageColor.Green);

            authorizationStatus = AuthorizationStatus.Succeed;
            ChatController.Instance.OnAuthorized();
        }

        protected override IServiceBase GetServiceBase()
        {
            return ServiceContainer.ChatService;
        }
    }
}