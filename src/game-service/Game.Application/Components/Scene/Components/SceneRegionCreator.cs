using Game.Application.Objects;
using Game.Log;
using InterestManagement;

namespace Game.Application.Components
{
    public class SceneRegionCreator : ComponentBase, ISceneRegionCreator
    {
        private readonly IMatrixRegion<IGameObject> region;

        public SceneRegionCreator(Vector2 sceneSize, Vector2 regionSize)
        {
            var log = new InterestManagementLog();

            region = new MatrixRegion<IGameObject>(sceneSize, regionSize, log);
        }

        protected override void OnRemoved()
        {
            region?.Dispose();
        }

        public IMatrixRegion<IGameObject> GetRegion()
        {
            return region;
        }
    }
}