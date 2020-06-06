using System.Collections.Generic;
using Common.ComponentModel;
using Common.MathematicsHelper;
using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneContainer : ComponentBase, IGameSceneContainer
    {
        private readonly IReadOnlyDictionary<Map, IScene<IGameObject>> container;

        public GameSceneContainer()
        {
            // TODO: Consider adding new scenes using method or OnAwake()
            container = new Dictionary<Map, IScene<IGameObject>>()
            {
                { Map.Lobby, new Scene<IGameObject>(Vector2.Zero, Vector2.Zero) },
                { Map.TheDarkForest, new Scene<IGameObject>(Vector2.Zero, Vector2.Zero) }
            };
        }

        protected override void OnRemoved()
        {
            foreach (var scene in container.Values)
            {
                scene?.Dispose();
            }
        }

        public bool TryGetScene(Map map, out IScene<IGameObject> scene)
        {
            return container.TryGetValue(map, out scene);
        }
    }
}