using System.Collections.Concurrent;
using System.Collections.Generic;
using Common.ComponentModel;
using Game.Application.Objects;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameObjectCollection : ComponentBase, IGameObjectCollection
    {
        private ConcurrentDictionary<int, IGameObject> collection;

        public GameObjectCollection(IEnumerable<IGameObject> gameObjects = null)
        {
            collection = new ConcurrentDictionary<int, IGameObject>();

            if (gameObjects != null)
            {
                foreach (var gameObject in gameObjects)
                {
                    collection.TryAdd(gameObject.Id, gameObject);
                }
            }
        }

        protected override void OnRemoved()
        {
            foreach (var gameObject in collection.Values)
            {
                gameObject?.Dispose();
            }

            collection.Clear();
        }

        public bool AddGameObject(IGameObject gameObject)
        {
            return collection.TryAdd(gameObject.Id, gameObject);
        }

        public void RemoveGameObject(int id)
        {
            if (collection.TryRemove(id, out var gameObject))
            {
                gameObject?.Dispose();
            }
        }

        public bool TryGetGameObject(int id, out IGameObject gameObject)
        {
            return collection.TryGetValue(id, out gameObject);
        }
    }
}