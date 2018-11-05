namespace Common.MathematicsHelper
{
    public interface IRectangle
    {
        /// <summary>
        /// Gets the position in the geometric world.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the size in the geometric world.
        /// </summary>
        Vector2 Size { get; }

        /// <summary>
        /// Determines if two geometric shapes intersect.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <returns>If intersects or not.</returns>
        bool Intersects(Vector2 position, Vector2 size);
    }
}