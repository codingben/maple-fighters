using System.Collections.Generic;
using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneCollection : ComponentBase, IGameSceneCollection
    {
        private readonly IDictionary<Map, IGameScene> collection;

        public GameSceneCollection()
        {
            collection = new Dictionary<Map, IGameScene>();
        }

        public void AddScene(Map map, IGameScene scene)
        {
            collection.Add(map, scene);
        }

        public void RemoveScene(Map map)
        {
            collection[map]?.Dispose();
            collection.Remove(map);
        }

        public bool TryGetScene(Map map, out IGameScene scene)
        {
            return collection.TryGetValue(map, out scene);
        }
    }
}