using Common.ComponentModel;

namespace Game.Application.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameSceneManager : ComponentBase
    {
        private IGameSceneCollection gameSceneCollection;

        protected override void OnAwake()
        {
            gameSceneCollection = Components.Get<IGameSceneCollection>();

            var lobby = CreateMap(Map.Lobby);
            var theDarkForest = CreateMap(Map.TheDarkForest);

            gameSceneCollection.Add(Map.Lobby, lobby);
            gameSceneCollection.Add(Map.TheDarkForest, theDarkForest);
        }

        protected override void OnRemoved()
        {
            gameSceneCollection.Remove(Map.Lobby);
            gameSceneCollection.Remove(Map.TheDarkForest);
        }

        private IGameScene CreateMap(Map map)
        {
            IGameScene gameScene = null;

            switch (map)
            {
                case Map.Lobby:
                {
                    gameScene = new Lobby();
                    break;
                }

                case Map.TheDarkForest:
                {
                    gameScene = new TheDarkForest();
                    break;
                }
            }

            return gameScene;
        }
    }
}