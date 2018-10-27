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
            var regions =
                MatrixRegion.GetRegions(
                    sceneObject.Transform.Rectangle.GetFixedCorners());

            foreach (var region in regions)
            {
                region.AddSceneObject(sceneObject);
            }
        }

        public void RemoveSceneObject(ISceneObject sceneObject)
        {
            var regions =
                MatrixRegion.GetRegions(
                    sceneObject.Transform.Rectangle.GetFixedCorners());

            foreach (var region in regions)
            {
                region.RemoveSceneObject(sceneObject);
            }
        }
    }
}