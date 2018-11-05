using System.Collections.Generic;

namespace Common.MathematicsHelper
{
    public struct Rectangle : IRectangle
    {
        public float X { get; private set; }

        public float Y { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public float Left => X;

        public float Top => Y;

        public float Right => X + Width;

        public float Bottom => Y + Height;

        public Vector2 Position => new Vector2(X, Y);

        public Vector2 Size => new Vector2(Width, Height);

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2 position, Vector2 size)
        {
            X = position.X;
            Y = position.Y;
            Width = size.X;
            Height = size.Y;
        }

        public void SetPosition(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
        }

        public void SetSize(Vector2 size)
        {
            Width = size.X;
            Height = size.Y;
        }

        public bool Intersects(Rectangle other)
        {
            return (other.X < X + Width) &&
                   (X < (other.X + other.Width)) &&
                   (other.Y < Y + Height) &&
                   (Y < other.Y + other.Height);
        }

        public bool Intersects(Vector2 position, Vector2 size)
        {
            return (position.X < X + Width) &&
                   (X < (position.X + size.X)) &&
                   (position.Y < Y + Height) &&
                   (Y < position.Y + size.Y);
        }

        public override string ToString()
        {
            return $"X: {X} Y: {Y} Width: {Width} Height: {Height}";
        }

        public Vector2[] GetVertices()
        {
            var topLeft = new Vector2(
                x: X + (-(Width / 2) + 0.1f),
                y: Y + ((Height / 2) - 0.1f));

            var topRight = new Vector2(
                x: X + ((Width / 2) - 0.1f),
                y: Y + ((Height / 2) - 0.1f));

            var bottomLeft = new Vector2(
                x: X + ((Width / 2) - 0.1f),
                y: Y + (-(Height / 2) + 0.1f));

            var bottomRight = new Vector2(
                x: X + (-(Width / 2) + 0.1f),
                y: Y + (-(Height / 2) + 0.1f));

            return new[] { topLeft, topRight, bottomLeft, bottomRight };
        }

        public static IEnumerable<Vector2> GetVertices(
            Vector2 position,
            Vector2 size)
        {
            var topLeft = new Vector2(
                x: position.X + (-(size.X / 2) + 0.1f),
                y: position.Y + ((size.Y / 2) - 0.1f));

            var topRight = new Vector2(
                x: position.X + ((size.X / 2) - 0.1f),
                y: position.Y + ((size.Y / 2) - 0.1f));

            var bottomLeft = new Vector2(
                x: position.X + ((size.X / 2) - 0.1f),
                y: position.Y + (-(size.Y / 2) + 0.1f));

            var bottomRight = new Vector2(
                x: position.X + (-(size.X / 2) + 0.1f),
                y: position.Y + (-(size.Y / 2) + 0.1f));

            return new[] { topLeft, topRight, bottomLeft, bottomRight };
        }
    }
}