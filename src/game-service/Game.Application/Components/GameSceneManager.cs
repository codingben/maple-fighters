using System;
using System.Collections.Concurrent;
using Common.ComponentModel;
using Common.Components;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneManager : ComponentBase, IGameSceneManager
    {
        private ConcurrentDictionary<Map, IGameScene> collection;

        public GameSceneManager()
        {
            collection = new ConcurrentDictionary<Map, IGameScene>();
        }

        protected override void OnAwake()
        {
            var idGenerator = Components.Get<IIdGenerator>();

            // TODO: Use another way to add game scenes
            collection.TryAdd(Map.Lobby, new LobbyGameScene(idGenerator));
            collection.TryAdd(Map.TheDarkForest, new TheDarkForestGameScene(idGenerator));
        }

        protected override void OnRemoved()
        {
            foreach (var gameScene in collection.Values)
            {
                gameScene?.Dispose();
                ((IDisposable)gameScene.Components)?.Dispose();
            }

            collection.Clear();
        }

        public bool TryGetGameScene(Map map, out IGameScene gameScene)
        {
            return collection.TryGetValue(map, out gameScene);
        }
    }
}