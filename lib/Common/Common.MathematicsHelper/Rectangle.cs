using System;

namespace Common.MathematicsHelper
{
    public struct Rectangle
    {
        public static readonly Rectangle Empty = new Rectangle();

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

        public override bool Equals(object other)
        {
            if (!(other is Rectangle))
            {
                return false;
            }

            var rectangle = (Rectangle)other;

            return (rectangle.X == X) &&
                   (rectangle.Y == Y) &&
                   (rectangle.Width == Width) &&
                   (rectangle.Height == Height);
        }

        public override int GetHashCode()
        {
            return (int)((uint)X ^
                        (((uint)Y << 13) | ((uint)Y >> 19)) ^
                            (((uint)Width << 26) | ((uint)Width >> 6)) ^
                                (((uint)Height << 7) | ((uint)Height >> 25)));
        }

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            var x1 = Math.Max(a.X, b.X);
            var x2 = Math.Min(a.X + a.Width, b.X + b.Width);
            var y1 = Math.Max(a.Y, b.Y);
            var y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

            if (x2 >= x1 && y2 >= y1)
            {
                return new Rectangle(x1, y1, x2 - x1, y2 - y1);
            }

            return Empty;
        }

        public bool IntersectsWith(Rectangle rectangle)
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
    }
}