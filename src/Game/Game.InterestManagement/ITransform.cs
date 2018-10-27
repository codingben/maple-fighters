using System;
using Common.MathematicsHelper;

namespace Game.InterestManagement
{
    public interface ITransform
    {
        /// <summary>
        /// Gets position and the size of the object.
        /// </summary>
        Rectangle Rectangle { get; }

        /// <summary>
        /// The notifier of the new position.
        /// </summary>
        event Action PositionChanged;

        /// <summary>
        /// The notifier of the new size.
        /// </summary>
        event Action SizeChanged;
    }
}