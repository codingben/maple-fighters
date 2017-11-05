using MathematicsHelper;
using Shared.Game.Common;

namespace Game.Application.Components
{
    public struct SpawnPositionDetails
    {
        public readonly Vector2 Position;
        public readonly Directions Direction;

        public SpawnPositionDetails(Vector2 position, Directions direction)
        {
            Position = position;
            Direction = direction;
        }
    }
}