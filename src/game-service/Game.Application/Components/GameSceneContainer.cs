using System.Collections.Generic;
using Common.ComponentModel;
using Common.MathematicsHelper;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneContainer : ComponentBase, IGameSceneContainer
    {
        private readonly IReadOnlyDictionary<Map, IGameScene> container;

        public GameSceneContainer()
        {
            var lobbyScene = new GameScene(worldSize: new Vector2(40, 5), regionSize: new Vector2(10, 5));
            lobbyScene.PlayerSpawnData = new PlayerSpawnData(Vector2.Zero, Vector2.Zero);

            var theDarkForestScene = new GameScene(worldSize: new Vector2(30, 30), regionSize: new Vector2(10, 5));
            theDarkForestScene.PlayerSpawnData = new PlayerSpawnData(Vector2.Zero, Vector2.Zero);

            // TODO: Consider adding new scenes using method or OnAwake()
            container = new Dictionary<Map, IGameScene>()
            {
                { Map.Lobby, lobbyScene },
                { Map.TheDarkForest, theDarkForestScene }
            };
        }

        protected override void OnRemoved()
        {
            foreach (var scene in container.Values)
            {
                scene?.Dispose();
            }
        }

        public bool TryGetScene(Map map, out IGameScene scene)
        {
            return container.TryGetValue(map, out scene);
        }
    }
}