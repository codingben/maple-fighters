using System;
using System.Collections.Generic;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public interface IMatrixRegion<TObject> : IDisposable
        where TObject : ISceneObject
    {
        /// <summary>
        /// Gets the regions by the scene object position.
        /// </summary>
        /// <param name="points">The points which overlaps with the regions.</param>
        /// <returns>The relevant regions.</returns>
        IEnumerable<IRegion<TObject>> GetRegions(IEnumerable<Vector2> points);

        /// <summary>
        /// Gets all the regions in the scene.
        /// </summary>
        /// <returns>The regions.</returns>
        IRegion<TObject>[,] GetAllRegions();
    }
}