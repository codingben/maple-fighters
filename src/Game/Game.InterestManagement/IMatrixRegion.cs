using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    /// <summary>
    /// Represents the matrix region inside the scene.
    /// </summary>
    public interface IMatrixRegion : IDisposable
    {
        /// <summary>
        /// Gets the regions by the scene object position.
        /// </summary>
        /// <param name="positions">The points which overlaps with the regions.</param>
        /// <returns>The relevant regions.</returns>
        IEnumerable<IRegion> GetRegions(IEnumerable<Vector2> positions);

        /// <summary>
        /// Gets all the regions in the scene.
        /// </summary>
        /// <returns>The regions.</returns>
        IRegion[,] GetAllRegions();
    }
}