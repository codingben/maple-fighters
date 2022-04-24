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

        int ProvideId();

        IWebSocketConnection ProvideConnection();
    }
}