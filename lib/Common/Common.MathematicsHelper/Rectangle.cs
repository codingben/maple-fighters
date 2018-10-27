using System.Collections.Generic;

namespace Common.MathematicsHelper
{
    public struct Rectangle
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

        public bool Intersects(Rectangle rectangle)
        {
            return (rectangle.X < X + Width) &&
                   (X < (rectangle.X + rectangle.Width)) &&
                   (rectangle.Y < Y + Height) &&
                   (Y < rectangle.Y + rectangle.Height);
        }

        public override string ToString()
        {
            return $"X: {X} Y: {Y} Width: {Width} Height: {Height}";
        }

        public IEnumerable<Vector2> GetFixedCorners()
        {
            var corners = GetCorners();

            for (var i = 0; i < corners.Length; i++)
            {
                corners[i].X += X;
                corners[i].Y += Y;
            }

            return corners;
        }

        public Vector2[] GetCorners()
        {
            var topLeft = new Vector2(
                x: -(Width / 2) + 0.1f,
                y: (Height / 2) - 0.1f);

            var topRight = new Vector2(
                x: (Width / 2) - 0.1f,
                y: (Height / 2) - 0.1f);

            var bottomLeft = new Vector2(
                x: (Width / 2) - 0.1f,
                y: -(Height / 2) + 0.1f);

            var bottomRight = new Vector2(
                x: -(Width / 2) + 0.1f,
                y: -(Height / 2) + 0.1f);

            return new[] { topLeft, topRight, bottomLeft, bottomRight };
        }
    }
}