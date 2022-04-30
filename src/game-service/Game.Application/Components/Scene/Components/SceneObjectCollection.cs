using System.Collections.Concurrent;
using System.Collections.Generic;
using Game.Application.Objects;

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
            return collection.TryAdd(gameObject.Id, gameObject);
        }

        public bool Remove(int id)
        {
            return collection.TryRemove(id, out _);
        }

        public bool TryGet(int id, out IGameObject gameObject)
        {
            return collection.TryGetValue(id, out gameObject);
        }

        public IEnumerable<IGameObject> GetAll()
        {
            return collection.Values;
        }
    }
}