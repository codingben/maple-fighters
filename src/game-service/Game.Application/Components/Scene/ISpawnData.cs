using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public interface ISpawnData
    {
        Vector2 Position { get; set; }

        Vector2 Size { get; set; }

        float Direction { get; set; }
    }
}