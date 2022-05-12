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
            IGameSceneCollection gameSceneCollection)
        {
            Components = new ComponentCollection();
            Components.Add(new WebSocketConnectionProvider(id, connection));
            Components.Add(new WebSocketConnectionErrorHandler());
            Components.Add(new RemotePlayerProvider());
            Components.Add(new RemotePlayerMessageSender());
            Components.Add(new MessageHandlerManager(gameSceneCollection));
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}