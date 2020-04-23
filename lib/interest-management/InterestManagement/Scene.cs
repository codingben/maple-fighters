using Common.MathematicsHelper;

namespace InterestManagement
{
    public class Scene<TObject> : IScene<TObject>
        where TObject : ISceneObject
    {
        public IMatrixRegion<TObject> MatrixRegion { get; }

        public Scene(Vector2 worldSize, Vector2 regionSize)
        {
            MatrixRegion =
                new MatrixRegion<TObject>(worldSize, regionSize);
        }

        public void Dispose()
        {
            MatrixRegion?.Dispose();
        }
    }
}