using System.Collections.Generic;

namespace Game.Application.Components
{
    public class GameClientCollection : ComponentBase, IGameClientCollection
    {
        private readonly Dictionary<int, IGameClient> collection;

        public GameClientCollection()
        {
            collection = new Dictionary<int, IGameClient>();
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

        public void Add(IGameClient gameClient)
        {
            var id =
                gameClient.Id;
            var webSocketConnectionProvider =
                gameClient.Components.Get<IWebSocketConnectionProvider>();
            var connection =
                webSocketConnectionProvider.ProvideConnection();
            connection.OnClose += () => Remove(id);

            collection.Add(id, gameClient);
        }

        public void Remove(int id)
        {
            if (collection.TryGetValue(id, out var gameClient))
            {
                gameClient.Dispose();
            }

            collection.Remove(id);
        }

        public bool TryGet(int id, out IGameClient gameClient)
        {
            return collection.TryGetValue(id, out gameClient);
        }
    }
}