using System;
using Game.Logger;

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
            GameLogger.Error(exception.Message);
        }
    }
}