using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PresenceSceneProvider : ComponentBase, IPresenceSceneProvider
    {
        private IScene<IGameObject> scene;

        public PresenceSceneProvider(IScene<IGameObject> scene)
        {
            this.scene = scene;
        }

        public void SetScene(IScene<IGameObject> scene)
        {
            this.scene = scene;
        }

        public IScene<IGameObject> GetScene()
        {
            return scene;
        }
    }
}