using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class GameSceneManager : ComponentBase
    {
        private IGameSceneContainer gameSceneContainer;

        protected override void OnAwake()
        {
            gameSceneContainer = Components.Get<IGameSceneContainer>();
            gameSceneContainer.AddScene(Map.Lobby, new LobbyGameScene());
            gameSceneContainer.AddScene(Map.TheDarkForest, new TheDarkForestGameScene());
        }

        protected override void OnRemoved()
        {
            gameSceneContainer.RemoveScene(Map.Lobby);
            gameSceneContainer.RemoveScene(Map.TheDarkForest);
        }
    }
}