using Game.MessageTools;
using Fleck;
using Game.Application.Objects.Components;
using Game.Application.Objects;

namespace Game.Application.Components
{
    public class RemotePlayerMessageSender : ComponentBase
    {
        private IGameObject remotePlayer;
        private IWebSocketConnection connection;
        private readonly IGameClientCollection gameClientCollection;

        public RemotePlayerMessageSender(IGameClientCollection gameClientCollection)
        {
            this.gameClientCollection = gameClientCollection;
        }

        protected override void OnAwake()
        {
            var remotePlayerProvider =
                Components.Get<IRemotePlayerProvider>();
            var webSocketConnectionProvider =
                Components.Get<IWebSocketConnectionProvider>();

            remotePlayer = remotePlayerProvider.Provide();
            connection = webSocketConnectionProvider.ProvideConnection();

            SetupMessageSender();
            SubscribeToMessageSender();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromMessageSender();
        }

        private void SetupMessageSender()
        {
            var messageSender = remotePlayer.Components.Get<IMessageSender>();
            messageSender.SetJsonSerializer(new NativeJsonSerializer());
        }

        private void SubscribeToMessageSender()
        {
            var messageSender = remotePlayer.Components.Get<IMessageSender>();
            messageSender.SendMessageCallback += OnSendMessageCallback;
            messageSender.SendToMessageCallback += OnSendMessageCallback;
        }

        private void UnsubscribeFromMessageSender()
        {
            var messageSender = remotePlayer.Components.Get<IMessageSender>();
            messageSender.SendMessageCallback -= OnSendMessageCallback;
            messageSender.SendToMessageCallback -= OnSendMessageCallback;
        }

        private void OnSendMessageCallback(string data)
        {
            if (connection.IsAvailable)
            {
                connection.Send(data);
            }
        }

        private void OnSendMessageCallback(string data, int id)
        {
            if (gameClientCollection.TryGet(id, out var gameClient))
            {
                var webSocketConnectionProvider =
                    gameClient.Components.Get<IWebSocketConnectionProvider>();
                var connection =
                    webSocketConnectionProvider.ProvideConnection();
                if (connection.IsAvailable)
                {
                    connection.Send(data);
                }
            }
        }
    }
}