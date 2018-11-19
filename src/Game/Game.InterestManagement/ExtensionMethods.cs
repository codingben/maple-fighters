using System.Collections.Generic;
using System.Linq;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public static class ExtensionMethods
    {
        public static IEnumerable<IRegion<TObject>> GetRegions<TObject>(
            this IMatrixRegion<TObject> matrixRegion,
            ITransform transform)
            where TObject : ISceneObject
        {
            var vertices = Rectangle.GetVertices(
                transform.Position,
                transform.Size);

            return matrixRegion.GetRegions(vertices);
        }

        public static IEnumerable<TObject> ExcludeSceneObject<TObject>(
            this IEnumerable<TObject> sceneObjects,
            TObject excludedSceneObject)
            where TObject : ISceneObject
        {
            return sceneObjects.Where(x => x.Id != excludedSceneObject.Id)
                .ToArray();
        }
    }
}