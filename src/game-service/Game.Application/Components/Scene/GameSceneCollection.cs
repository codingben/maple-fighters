using System.Collections.Generic;
using Game.Log;

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
            var gameScenes = collection.Values;

            foreach (var gameScene in gameScenes)
            {
                gameScene.Dispose();
            }

            collection.Clear();
        }

        public bool Add(string name, IGameScene gameScene)
        {
            var isAdded = collection.TryAdd(name, gameScene);
            if (isAdded)
            {
                GameLog.Debug($"Scene {name} added.");
            }

            return isAdded;
        }

        public bool Remove(string name)
        {
            var isRemoved = collection.Remove(name, out _);
            if (isRemoved)
            {
                GameLog.Debug($"Scene {name} removed.");
            }

            return isRemoved;
        }

        public bool TryGet(string name, out IGameScene gameScene)
        {
            return collection.TryGetValue(name, out gameScene);
        }

        public IEnumerable<IGameScene> GetAll()
        {
            return collection.Values;
        }
    }
}