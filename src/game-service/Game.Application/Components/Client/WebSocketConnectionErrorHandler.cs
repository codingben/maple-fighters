using System;
using Game.Log;

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
            GameLog.Error(exception.Message);
        }
    }
}