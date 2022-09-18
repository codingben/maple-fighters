using System.Collections.Concurrent;
using System.Collections.Generic;
using Game.Application.Objects;
using Game.Log;

namespace Game.Application.Components
{
    public class SceneObjectCollection : ComponentBase, ISceneObjectCollection
    {
        private readonly ConcurrentDictionary<int, IGameObject> collection;

        public SceneObjectCollection()
        {
            collection = new ConcurrentDictionary<int, IGameObject>();
        }

        protected override void OnRemoved()
        {
            var gameObjects = collection.Values;

            foreach (var gameObject in gameObjects)
            {
                gameObject.Dispose();
            }

            collection.Clear();
        }

        public bool Add(IGameObject gameObject)
        {
            var isAdded = collection.TryAdd(gameObject.Id, gameObject);
            if (isAdded)
            {
                GameLog.Debug($"Game object {gameObject.Name} added.");
            }

            return isAdded;
        }

        public bool Remove(int id)
        {
            var isRemoved = collection.TryRemove(id, out _);
            if (isRemoved)
            {
                GameLog.Debug($"Game object #{id} removed.");
            }

            return isRemoved;
        }

        public bool TryGet(int id, out IGameObject gameObject)
        {
            return collection.TryGetValue(id, out gameObject);
        }

        public bool Exists(int id)
        {
            return collection.ContainsKey(id);
        }

        public IEnumerable<IGameObject> GetAll()
        {
            return collection.Values;
        }
    }
}