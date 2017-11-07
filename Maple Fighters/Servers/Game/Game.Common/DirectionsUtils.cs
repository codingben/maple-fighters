namespace Shared.Game.Common
{
    public static class DirectionsUtils
    {
        public static Directions ToDirections(this float direction)
        {
            return direction > 0 ? Directions.Left : Directions.Right;
        }

        public static float FromDirections(this Directions directions)
        {
            return directions == Directions.Left ? 1 : -1;
        }
    }
}