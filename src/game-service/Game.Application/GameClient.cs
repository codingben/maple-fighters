using System;
using Game.Application.Components;
using Fleck;

namespace Game.Application
{
    public class GameClient : IDisposable
    {
        private IComponents Components { get; }

        public GameClient(
            int id,
            IWebSocketConnection connection,
            IWebSocketSessionCollection webSocketSessionCollection,
            IGameSceneCollection gameSceneCollection)
        {
            Components = new ComponentCollection();
            Components.Add(new WebSocketConnectionProvider(id, connection, webSocketSessionCollection));
            Components.Add(new WebSocketConnectionErrorHandler());
            Components.Add(new RemotePlayerProvider());
            Components.Add(new RemotePlayerMessageSender(webSocketSessionCollection));
            Components.Add(new MessageHandlerManager(gameSceneCollection));
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}