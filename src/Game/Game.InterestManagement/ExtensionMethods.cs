using System.Collections.Generic;
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
    }
}