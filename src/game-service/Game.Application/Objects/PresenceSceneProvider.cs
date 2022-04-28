using System;
using Game.Application.Components;

namespace Game.Application.Objects
{
    public class PresenceSceneProvider : ComponentBase, IPresenceSceneProvider
    {
        public event Action<IGameScene> SceneChanged;

        private IGameScene gameScene;
        private IProximityChecker proximityChecker;

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();

            SetRegion();
        }

        public void SetScene(IGameScene gameScene)
        {
            this.gameScene = gameScene;

            SetRegion();

            SceneChanged?.Invoke(gameScene);
        }

        private void SetRegion()
        {
            var sceneRegionCreator =
                gameScene?.Components.Get<ISceneRegionCreator>();
            if (sceneRegionCreator != null)
            {
                var region = sceneRegionCreator.GetRegion();

                proximityChecker.SetMatrixRegion(region);
            }
        }

        public IGameScene GetScene()
        {
            return gameScene;
        }
    }
}