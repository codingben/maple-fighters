using System;
using Common.MathematicsHelper;

namespace InterestManagement
{
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

        /// <summary>
        /// Sets the new position to the object.
        /// </summary>
        /// <param name="position">The position.</param>
        void SetPosition(Vector2 position);

        /// <summary>
        /// Sets the new size to the object.
        /// </summary>
        /// <param name="size">The new size.</param>
        void SetSize(Vector2 size);
    }
}