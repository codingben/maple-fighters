using System;
using Fleck;
using Game.MessageTools;

namespace Game.Application.Components
{
    public class WebSocketConnectionProvider : ComponentBase, IWebSocketConnectionProvider
    {
        public event Action ConnectionEstablished;

        public event Action ConnectionClosed;

        public event Action<Exception> ErrorOccurred;

        public event Action<string> MessageReceived;

        private readonly IWebSocketConnection connection;
        private readonly IJsonSerializer jsonSerializer = new NativeJsonSerializer();

        public WebSocketConnectionProvider(IWebSocketConnection connection)
        {
            this.connection = connection;
            this.connection.OnOpen += OnConnectionEstablished;
            this.connection.OnClose += OnConnectionClosed;
            this.connection.OnError += OnErrorOccurred;
            this.connection.OnMessage += OnMessageReceived;
        }

        public void SendMessage<T>(byte code, T message)
            where T : struct
        {
            var data = jsonSerializer.Serialize(new MessageData()
            {
                Code = code,
                Data = jsonSerializer.Serialize(message)
            });

            if (connection.IsAvailable)
            {
                connection.Send(data);
            }
        }

        private void OnConnectionEstablished()
        {
            ConnectionEstablished?.Invoke();
        }

        private void OnConnectionClosed()
        {
            ConnectionClosed?.Invoke();
        }

        private void OnErrorOccurred(Exception exception)
        {
            ErrorOccurred?.Invoke(exception);
        }

        private void OnMessageReceived(string message)
        {
            MessageReceived?.Invoke(message);
        }

        public IWebSocketConnection ProvideConnection()
        {
            return connection;
        }
    }
}