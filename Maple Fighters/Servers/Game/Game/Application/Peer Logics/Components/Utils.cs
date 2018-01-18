using Game.InterestManagement;
using Shared.Game.Common;

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