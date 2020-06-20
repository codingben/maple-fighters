using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class GameSceneManager : ComponentBase
    {
        private IGameSceneCollection gameSceneCollection;

        protected override void OnAwake()
        {
            gameSceneCollection = Components.Get<IGameSceneCollection>();
            gameSceneCollection.AddScene(Map.Lobby, new LobbyGameScene());
            gameSceneCollection.AddScene(Map.TheDarkForest, new TheDarkForestGameScene());
        }

        protected override void OnRemoved()
        {
            gameSceneCollection.RemoveScene(Map.Lobby);
            gameSceneCollection.RemoveScene(Map.TheDarkForest);
        }
    }
}