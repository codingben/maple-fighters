using System;

namespace Game.InterestManagement
{
    public interface IScene<TObject> : IDisposable
        where TObject : ISceneObject
    {
        /// <summary>
        /// Gets the access to the scene regions.
        /// </summary>
        IMatrixRegion<TObject> MatrixRegion { get; }
    }
}