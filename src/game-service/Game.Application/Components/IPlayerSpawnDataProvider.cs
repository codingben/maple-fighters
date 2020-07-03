using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public interface IPlayerSpawnDataProvider
    {
        Vector2 GetPosition();

        Vector2 GetSize();
    }
}