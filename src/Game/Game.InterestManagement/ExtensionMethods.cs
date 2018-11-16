using System.Collections.Generic;
using System.Linq;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public static class ExtensionMethods
    {
        public static IEnumerable<IRegion> GetRegions(
            this IMatrixRegion matrixRegion,
            ITransform transform)
        {
            var vertices = Rectangle.GetVertices(
                transform.Position,
                transform.Size);

            return matrixRegion.GetRegions(vertices);
        }

        public static IEnumerable<ISceneObject> ExcludeSceneObject(
            this IEnumerable<ISceneObject> sceneObjects,
            ISceneObject excludedSceneObject)
        {
            return sceneObjects.Where(x => x.Id != excludedSceneObject.Id)
                .ToArray();
        }
    }
}