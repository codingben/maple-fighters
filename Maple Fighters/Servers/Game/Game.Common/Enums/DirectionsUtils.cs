namespace Shared.Game.Common
{
    public static class DirectionsUtils
    {
        public static Directions ToDirections(this sbyte direction)
        {
            return direction > 0 ? Directions.Left : Directions.Right;
        }

        public static byte FromDirections(this Directions directions)
        {
            return (byte)(directions == Directions.Left ? 1 : -1);
        }
    }
}