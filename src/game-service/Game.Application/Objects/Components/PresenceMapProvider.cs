using Common.ComponentModel;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class PresenceMapProvider : ComponentBase, IPresenceMapProvider
    {
        private IGameScene gameScene;
        private IProximityChecker proximityChecker;

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();
        }

        public void SetMap(IGameScene gameScene)
        {
            this.gameScene = gameScene;

            var region = gameScene.MatrixRegion;
            proximityChecker.SetMatrixRegion(region);
        }

        public IGameScene GetMap()
        {
            return gameScene;
        }
    }
}