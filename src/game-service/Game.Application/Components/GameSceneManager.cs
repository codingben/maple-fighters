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

        protected override void OnAwake()
        {
            var idGenerator = Components.Get<IIdGenerator>();

            collection = new ConcurrentDictionary<Map, IGameScene>();
            collection.TryAdd(Map.Lobby, new LobbyGameScene(idGenerator));
            collection.TryAdd(Map.TheDarkForest, new TheDarkForestGameScene(idGenerator));
        }

        protected override void OnRemoved()
        {
            var lobby = collection[Map.Lobby];
            lobby.Dispose();
            ((IDisposable)lobby.Components).Dispose();

            var TheDarkForest = collection[Map.TheDarkForest];
            TheDarkForest.Dispose();
            ((IDisposable)TheDarkForest.Components).Dispose();

            collection.Clear();
        }

        public bool TryGetGameScene(Map map, out IGameScene gameScene)
        {
            return collection.TryGetValue(map, out gameScene);
        }
    }
}