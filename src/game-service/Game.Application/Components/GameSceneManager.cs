using System.Collections.Generic;
using Common.ComponentModel;
using Common.Components;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class GameSceneManager : ComponentBase
    {
        private Dictionary<Map, IGameScene> collection;

        protected override void OnAwake()
        {
            var idGenerator = Components.Get<IIdGenerator>();

            collection = new Dictionary<Map, IGameScene>();
            collection.Add(Map.Lobby, new LobbyGameScene(idGenerator));
            collection.Add(Map.TheDarkForest, new TheDarkForestGameScene(idGenerator));
        }

        protected override void OnRemoved()
        {
            collection[Map.Lobby].Dispose();
            collection[Map.TheDarkForest].Dispose();
            collection.Clear();
        }
    }
}