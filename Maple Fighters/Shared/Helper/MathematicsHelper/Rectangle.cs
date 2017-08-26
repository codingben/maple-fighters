using System;

namespace MathematicsHelper
{
    public struct Rectangle : IEquatable<Rectangle>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public int Left => X;
        public int Top => Y;
        public int Right => X + Width;
        public int Bottom => Y + Height;

        public Vector2 Position => new Vector2(X, Y);
        public Vector2 Size => new Vector2(Width, Height);

        public static readonly Rectangle EMPTY = new Rectangle(Vector2.Zero, Vector2.Zero);

        public Rectangle(Vector2 position, Vector2 size)
        {
            X = (int)position.X;
            Y = (int)position.Y;
            Width = (int)size.X;
            Height = (int)size.Y;
        }

        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString() => $"X={X}, Y={Y}, Width={Width}, Height={Height}";

        public override int GetHashCode()
        {
            return unchecked((int)((uint)X ^
                                   (((uint)Y << 13) | ((uint)Y >> 19)) ^
                                   (((uint)Width << 26) | ((uint)Width >> 6)) ^
                                   (((uint)Height << 7) | ((uint)Height >> 25))));
        }

        public override bool Equals(object obj) => obj is Rectangle && Equals((Rectangle)obj);

        public bool Equals(Rectangle other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Width == other.Width &&
                   Height == other.Height;
        }
        
        public bool Contains(Vector2 position)
        {
            return position.X >= X &&
                   position.X < Right &&
                   position.Y >= Y &&
                   position.Y < Bottom;
        }

        public bool Contains(Rectangle other)
        {
            return other.X >= X &&
                   other.Right <= Right &&
                   other.Y >= Y &&
                   other.Bottom <= Bottom;
        }

        public Rectangle Intersect(Rectangle other)
        {
            var biggestX = Math.Max(X, other.X);
            var smallestRight = Math.Min(Right, other.Right);
            var biggestY = Math.Max(Y, other.Y);
            var smallestBottom = Math.Min(Bottom, other.Bottom);

            if (smallestRight >= biggestX && smallestBottom >= biggestY)
            {
                return new Rectangle(biggestX, biggestY, smallestRight - biggestX, smallestBottom - biggestY);
            }

            return EMPTY;
        }

        public bool IntersectsWith(Rectangle other)
        {
            return other.X < Right &&
                   X < other.Right &&
                   other.Y < Bottom &&
                   Y < other.Bottom;
        }
    }
}