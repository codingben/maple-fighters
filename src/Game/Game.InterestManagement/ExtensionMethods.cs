using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public static class ExtensionMethods
    {
        public static IEnumerable<IRegion<TObject>> GetRegions<TObject>(
            this IMatrixRegion<TObject> @this,
            ITransform transform)
            where TObject : ISceneObject
        {
            var vertices = Rectangle.GetVertices(
                transform.Position,
                transform.Size);

            return @this.GetRegions(vertices);
        }
    }
}