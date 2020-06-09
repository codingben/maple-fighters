using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public struct PlayerSpawnData
    {
        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public PlayerSpawnData(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
    }
}