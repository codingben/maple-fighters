using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public interface IGamePlayerSpawnData
    {
        void SetSpawnPosition(Vector2 position);

        Vector2 GetSpawnPosition();
    }
}