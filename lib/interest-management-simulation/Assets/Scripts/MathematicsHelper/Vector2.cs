using System;

namespace MathematicsHelper
{
    public struct Vector2 : IEquatable<Vector2>
    {
        public static Vector2 Zero => new Vector2(0, 0);

        public static Vector2 One => new Vector2(1, 1);

        public static Vector2 Up => new Vector2(0, 1);

        public static Vector2 Down => new Vector2(0, -1);

        public static Vector2 Left => new Vector2(-1, 0);

        public static Vector2 Right => new Vector2(1, 0);

        public float X { get; private set; }

        public float Y { get; private set; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2 Normalize()
        {
            var value = 1.0f / (float)Math.Sqrt((X * X) + (Y * Y));

            X *= value;
            Y *= value;

            return new Vector2(X, Y);
        }

        public static float Distance(Vector2 a, Vector2 b)
        {
            var x = a.X - b.X;
            var y = a.Y - b.Y;

            return (float)Math.Sqrt((x * x) + (y * y));
        }

        public static void Distance(
            ref Vector2 a,
            ref Vector2 b,
            out float result)
        {
            var x = a.X - b.X;
            var y = a.Y - b.Y;

            result = (float)Math.Sqrt((x * x) + (y * y));
        }

        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            return new Vector2(
                a.X > b.X ? a.X : b.X,
                a.Y > b.Y ? a.Y : b.Y);
        }

        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            return new Vector2(
                a.X < b.X ? a.X : b.X,
                a.Y < b.Y ? a.Y : b.Y);
        }

        public override string ToString()
        {
            return $"X: {X} Y: {Y}";
        }

        public override bool Equals(object other)
        {
            return other is Vector2 && Equals(this);
        }

        public bool Equals(Vector2 other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        public float Length()
        {
            return (float)Math.Sqrt((X * X) + (Y * Y));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static Vector2 operator -(Vector2 value)
        {
            value.X = -value.X;
            value.Y = -value.Y;

            return value;
        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            a.X += b.X;
            a.Y += b.Y;

            return a;
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            a.X -= b.X;
            a.Y -= b.Y;

            return a;
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            a.X *= b.X;
            a.Y *= b.Y;

            return a;
        }

        public static Vector2 operator *(Vector2 value, float scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;

            return value;
        }

        public static Vector2 operator *(float scaleFactor, Vector2 value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;

            return value;
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            a.X /= b.X;
            a.Y /= b.Y;

            return a;
        }

        public static Vector2 operator /(Vector2 value, float divider)
        {
            var factor = 1 / divider;

            value.X *= factor;
            value.Y *= factor;

            return value;
        }
    }
}