using Game.Common;
using InterestManagement;

namespace Game.Application.PeerLogic.Components
{
    public static class Utils
    {
        public static Directions GetDirectionsFromDirection(this Direction directions)
        {
            return directions == Direction.Left ? Directions.Left : Directions.Right;
        }
    }
}