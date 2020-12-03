using System;

namespace InterestManagement
{
    public interface IScene<TSceneObject> : IDisposable
        where TSceneObject : ISceneObject
    {
        /// <summary>
        /// Gets the access to the scene regions.
        /// </summary>
        IMatrixRegion<TSceneObject> MatrixRegion { get; }
    }
}