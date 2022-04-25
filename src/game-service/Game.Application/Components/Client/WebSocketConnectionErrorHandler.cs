using System;

namespace Game.Application.Components
{
    public class WebSocketConnectionErrorHandler : ComponentBase
    {
        protected override void OnAwake()
        {
            var webSocketConnectionProvider =
                Components.Get<IWebSocketConnectionProvider>();
            webSocketConnectionProvider.ErrorOccurred += OnErrorOccurred;
        }

        private void OnErrorOccurred(Exception exception)
        {
            Console.WriteLine($"{exception.Message}");
        }
    }
}