using System;
using Fleck;

namespace Game.Application.Components
{
    public interface IWebSocketConnectionProvider
    {
        event Action ConnectionEstablished;

        event Action ConnectionClosed;

        event Action<Exception> ErrorOccurred;

        event Action<string> MessageReceived;

        void SendMessage<T>(byte code, T message)
            where T : struct;

        IWebSocketConnection ProvideConnection();
    }
}