using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public interface IGamePlayerSpawnData
    {
        void SetSpawnPosition(Vector2 position);

        void SetSpawnDirection(float direction);

        Vector2 GetSpawnPosition();

        float GetSpawnDirection();
    }
}