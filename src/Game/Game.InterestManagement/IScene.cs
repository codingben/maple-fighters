using System;

namespace Game.InterestManagement
{
    public interface IScene : IDisposable
    {
        /// <summary>
        /// Gets the access to the scene regions.
        /// </summary>
        IMatrixRegion MatrixRegion { get; }
    }
}