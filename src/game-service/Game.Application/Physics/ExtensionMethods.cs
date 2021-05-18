using Box2DX.Common;
using Common.MathematicsHelper;

namespace Game.Physics
{
    public static class ExtensionMethods
    {
        public static Vector2 ToVector2(this Vec2 vec2)
        {
            return new Vector2(vec2.X, vec2.Y);
        }

        public static Vec2 FromVector2(this Vector2 vector2)
        {
            return new Vec2(vector2.X, vector2.Y);
        }
    }
}