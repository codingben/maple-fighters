using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public interface IPlayerSpawnData
    {
        Vector2 GetPosition();

        Vector2 GetSize();
    }
}