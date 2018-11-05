using System;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    /// <summary>
    /// The transform in the scene of the scene object.
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// The notifier of the new position.
        /// </summary>
        event Action PositionChanged;

        /// <summary>
        /// Gets position of the object.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets size of the object.
        /// </summary>
        Vector2 Size { get; }
    }
}