using System.Collections.Concurrent;
using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneCollection : ComponentBase, IGameSceneCollection
    {
        private ConcurrentDictionary<Map, IGameScene> collection;

        public GameSceneCollection()
        {
            collection = new ConcurrentDictionary<Map, IGameScene>();
        }

        public bool Add(Map map, IGameScene gameScene)
        {
            return collection.TryAdd(map, gameScene);
        }

        public bool Remove(Map map)
        {
            return collection.TryRemove(map, out _);
        }

        public bool TryGet(Map map, out IGameScene gameScene)
        {
            return collection.TryGetValue(map, out gameScene);
        }
    }
}