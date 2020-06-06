using System.Collections.Generic;
using Common.ComponentModel;
using Common.MathematicsHelper;
using InterestManagement;

namespace Game.Application.Components
{
    public enum Map
    {
        Lobby,
        TheDarkForest
    }

    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneContainer : ComponentBase, IGameSceneContainer
    {
        private readonly IReadOnlyDictionary<Map, IScene<ISceneObject>> container;

        public GameSceneContainer()
        {
            container = new Dictionary<Map, IScene<ISceneObject>>()
            {
                { Map.Lobby, new Scene<ISceneObject>(Vector2.Zero, Vector2.Zero) },
                { Map.TheDarkForest, new Scene<ISceneObject>(Vector2.Zero, Vector2.Zero) }
            };
        }

        public bool TryGetScene(Map map, out IScene<ISceneObject> scene)
        {
            return container.TryGetValue(map, out scene);
        }
    }
}