using Common.ComponentModel;
using Game.Application.Components;

namespace Game.Application.Objects.Components
{
    public class PresenceMapProvider : ComponentBase, IPresenceMapProvider
    {
        private IGameScene gameScene;
        private IProximityChecker proximityChecker;

        public PresenceMapProvider(IGameScene gameScene = null)
        {
            this.gameScene = gameScene;
        }

        protected override void OnAwake()
        {
            proximityChecker = Components.Get<IProximityChecker>();

            SetRegion();
        }

        public void SetMap(IGameScene gameScene)
        {
            this.gameScene = gameScene;

            SetRegion();
        }

        private void SetRegion()
        {
            var region = gameScene?.MatrixRegion;
            if (region != null)
            {
                proximityChecker.SetMatrixRegion(region);
            }
        }

        public IGameScene GetMap()
        {
            return gameScene;
        }
    }
}