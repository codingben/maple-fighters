using System;
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

        public GameObjectCollection(IEnumerable<IGameObject> gameObjects)
        {
            collection = new ConcurrentDictionary<int, IGameObject>();

            foreach (var gameObject in gameObjects)
            {
                collection.TryAdd(gameObject.Id, gameObject);
            }
        }

        protected override void OnRemoved()
        {
            foreach (var gameObject in collection.Values)
            {
                ((IDisposable)gameObject.Components)?.Dispose();
            }

            collection.Clear();
        }

        public bool TryGetGameObject(int id, out IGameObject gameObject)
        {
            return collection.TryGetValue(id, out gameObject);
        }
    }
}