namespace Assets.Scripts.Game
{
    using UnityVector2 = UnityEngine.Vector2;
    using UnityVector3 = UnityEngine.Vector3;
    using Vector2 = Common.MathematicsHelper.Vector2;

    public static class ExtensionMethods
    {
        public static Vector2 ToVector2(this UnityVector3 other)
        {
            return new Vector2(other.x, other.y);
        }

        public static Vector2 ToVector2(this UnityVector2 other)
        {
            return new Vector2(other.x, other.y);
        }

        public static UnityVector2 FromVector2(this Vector2 other)
        {
            return new UnityVector2(other.X, other.Y);
        }
    }
}