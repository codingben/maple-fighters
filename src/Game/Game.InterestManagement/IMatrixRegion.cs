using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IMatrixRegion
    {
        IEnumerable<IRegion> GetRegions(IEnumerable<Vector2> positions);
    }
}