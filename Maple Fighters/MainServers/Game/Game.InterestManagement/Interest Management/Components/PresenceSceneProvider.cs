using System;
using ComponentModel.Common;
using InterestManagement.Components.Interfaces;

namespace InterestManagement.Components
{
    public class PresenceSceneProvider : Component<ISceneObject>, IPresenceSceneProvider, IPresenceSceneChangesNotifier
    {
        public event Action<IScene> SceneChanged;
        private IScene scene;

        public PresenceSceneProvider(IScene scene = null) => this.scene = scene;

        public void SetScene(IScene scene)
        {
            this.scene = scene;

            SceneChanged?.Invoke(scene);
        }

        public IScene GetScene() => scene;
    }
}