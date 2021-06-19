using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public class PlayerSpawnData : ISpawnData
    {
        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; }

        public float Direction { get; set; }
    }
}