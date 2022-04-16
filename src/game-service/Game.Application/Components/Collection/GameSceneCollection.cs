using System.Collections.Generic;

namespace Game.Application.Components
{
    public class GameSceneCollection : ComponentBase, IGameSceneCollection
    {
        private readonly Dictionary<string, IGameScene> collection;

        public GameSceneCollection()
        {
            collection = new Dictionary<string, IGameScene>();
        }

        public void Dispose()
        {
            foreach (var item in collection.Values)
            {
                item.Dispose();
            }

            collection.Clear();
        }

        public bool Add(string name, IGameScene gameScene)
        {
            return collection.TryAdd(name, gameScene);
        }

        public void Remove(string name)
        {
            collection.Remove(name, out _);
        }

        public bool TryGet(string name, out IGameScene gameScene)
        {
            return collection.TryGetValue(name, out gameScene);
        }
    }
}