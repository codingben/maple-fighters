﻿using System;

namespace InterestManagement
{
    public interface ITransform
    {
        /// <summary>
        /// Occurs when a position has changed.
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
        /// Gets direction of the object.
        /// </summary>
        Vector2 Direction { get; }

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

        /// <summary>
        /// Sets the direction of the object.
        /// </summary>
        /// <param name="direction">The direction.</param>
        void SetDirection(Vector2 direction);
    }
}