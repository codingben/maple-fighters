using System;
using System.Collections.Generic;
using MathematicsHelper;

namespace InterestManagement
{
    public interface IMatrixRegion<TSceneObject> : IDisposable
        where TSceneObject : ISceneObject
    {
        /// <summary>
        /// Gets the regions by the scene object position.
        /// </summary>
        /// <param name="points">The points which overlaps with the regions.</param>
        /// <returns>The relevant regions.</returns>
        IEnumerable<IRegion<TSceneObject>> GetRegions(IEnumerable<Vector2> points);

        /// <summary>
        /// Gets all the regions in the scene.
        /// </summary>
        /// <returns>The regions.</returns>
        IRegion<TSceneObject>[,] GetAllRegions();
    }
}