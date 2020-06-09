using Common.ComponentModel;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PresenceSceneProvider : ComponentBase, IPresenceSceneProvider
    {
        private IGameScene scene;

        public PresenceSceneProvider(IGameScene scene)
        {
            this.scene = scene;
        }

        public void SetScene(IGameScene scene)
        {
            this.scene = scene;
        }

        public IGameScene GetScene()
        {
            return scene;
        }
    }
}