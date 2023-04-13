using System.Collections.Generic;
using Game.Log;

namespace Game.Application.Components
{
    public class GameClientCollection : ComponentBase, IGameClientCollection
    {
        private readonly Dictionary<int, IGameClient> collection;
        private readonly int maxNumberOfClients;

        public GameClientCollection(int maxNumberOfClients)
        {
            this.maxNumberOfClients = maxNumberOfClients;

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

        public bool Add(IGameClient gameClient)
        {
            if (Count() >= maxNumberOfClients)
            {
                GameLog.Debug($"Server reached maximum number of clients.");
                return false;
            }

            var id =
                gameClient.Id;
            var webSocketConnectionProvider =
                gameClient.Components.Get<IWebSocketConnectionProvider>();
            var connection =
                webSocketConnectionProvider.ProvideConnection();
            connection.OnClose += () => Remove(id);

            collection.Add(id, gameClient);

            GameLog.Debug($"Client #{id} connected.");
            return true;
        }

        public void Remove(int id)
        {
            if (collection.TryGetValue(id, out var gameClient))
            {
                gameClient.Dispose();
            }

            collection.Remove(id);

            GameLog.Debug($"Client #{id} disconnected.");
        }

        public bool TryGet(int id, out IGameClient gameClient)
        {
            return collection.TryGetValue(id, out gameClient);
        }

        public int Count()
        {
            return collection.Count;
        }
    }
}