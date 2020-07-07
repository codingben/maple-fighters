using System.Collections.Concurrent;
using Common.ComponentModel;
using Game.Application.Objects;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameObjectCollection : ComponentBase, IGameObjectCollection
    {
        private ConcurrentDictionary<int, IGameObject> collection;

        public GameObjectCollection()
        {
            collection = new ConcurrentDictionary<int, IGameObject>();
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
    }
}