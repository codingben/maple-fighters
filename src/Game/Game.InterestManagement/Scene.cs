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

        public void Dispose()
        {
            MatrixRegion?.Dispose();
        }

        public void AddSceneObject(ISceneObject sceneObject)
        {
            // TODO: Implement
        }

        public void RemoveSceneObject(ISceneObject sceneObject)
        {
            // TODO: Implement
        }
    }
}