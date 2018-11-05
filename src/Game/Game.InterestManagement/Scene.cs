using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class Scene : IScene
    {
        public IMatrixRegion MatrixRegion { get; }

        public Scene(Vector2 sceneSize, Vector2 regionSize)
        {
            MatrixRegion = new MatrixRegion(sceneSize, regionSize);
        }

        public void AddSceneObject(ISceneObject sceneObject)
        {
            var transform = sceneObject.Transform;
            var regions = MatrixRegion.GetRegions(transform);

            foreach (var region in regions)
            {
                region.Subscribe(sceneObject);
            }
        }

        public void RemoveSceneObject(ISceneObject sceneObject)
        {
            var transform = sceneObject.Transform;
            var regions = MatrixRegion.GetRegions(transform);

            foreach (var region in regions)
            {
                region.Unsubscribe(sceneObject);
            }
        }

        public void Dispose()
        {
            MatrixRegion?.Dispose();
        }
    }
}