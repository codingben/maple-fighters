using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public class Scene : IScene
    {
        public IMatrixRegion MatrixRegion { get; }

        public Scene(Vector2 worldSize, Vector2 regionSize)
        {
            MatrixRegion = new MatrixRegion(worldSize, regionSize);
        }

        public void Dispose()
        {
            MatrixRegion?.Dispose();
        }
    }
}