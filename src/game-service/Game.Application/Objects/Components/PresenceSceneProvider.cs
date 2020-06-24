using Common.ComponentModel;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PresenceSceneProvider : ComponentBase, IPresenceSceneProvider
    {
        private IGameScene gameScene;

        public PresenceSceneProvider(IGameScene gameScene)
        {
            this.gameScene = gameScene;
        }

        public void SetScene(IGameScene gameScene)
        {
            this.gameScene = gameScene;
        }

        public IGameScene GetScene()
        {
            return gameScene;
        }
    }
}