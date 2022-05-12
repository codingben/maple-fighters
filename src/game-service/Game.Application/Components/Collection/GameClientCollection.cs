using Fleck;
using System.Collections.Generic;

namespace Game.Application.Components
{
    public class GameClientCollection : ComponentBase, IGameClientCollection
    {
        private readonly Dictionary<int, GameClient> collection;

        private IIdGenerator idGenerator;
        private IGameSceneCollection gameSceneCollection;

        public GameClientCollection()
        {
            collection = new Dictionary<int, GameClient>();
        }

        protected override void OnAwake()
        {
            idGenerator = Components.Get<IIdGenerator>();
            gameSceneCollection = Components.Get<IGameSceneCollection>();
        }

        protected override void OnRemoved()
        {
            var gameClients = collection.Values;

            foreach (var gameClient in gameClients)
            {
                gameClient.Dispose();
            }

            collection.Clear();
        }

        public void Add(IWebSocketConnection connection)
        {
            var id = idGenerator.GenerateId();
            var gameClient = new GameClient(
                id,
                connection,
                gameSceneCollection);

            collection.Add(id, gameClient);

            AddConnectionClosedHandler(id, connection);
        }

        private void AddConnectionClosedHandler(int id, IWebSocketConnection connection)
        {
            connection.OnClose += () =>
            {
                if (collection.TryGetValue(id, out var gameClient))
                {
                    gameClient.Dispose();
                }

                collection.Remove(id);
            };
        }
    }
}