using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public class PlayerSpawnData
    {
        public Vector2 Position { get; }

        public Vector2 Size { get; }

        public PlayerSpawnData(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
    }
}