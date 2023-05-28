using Game.Application.Components;
using Fleck;

namespace Game.Application
{
    public class GameClient : IGameClient
    {
        public int Id { get; }

        public IComponents Components { get; }

        public GameClient(
            int id,
            IWebSocketConnection connection,
            IGameClientCollection gameClientCollection,
            IGameSceneCollection gameSceneCollection)
        {
            Id = id;
            Components = new ComponentCollection();
            Components.Add(new WebSocketConnectionProvider(connection));
            Components.Add(new WebSocketConnectionErrorHandler());
            Components.Add(new RemotePlayerProvider(id));
            Components.Add(new RemotePlayerMessageSender(gameClientCollection));
            Components.Add(new MessageHandlerManager(gameClientCollection, gameSceneCollection));
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}