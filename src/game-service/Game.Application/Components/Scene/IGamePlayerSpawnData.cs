using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public interface IGamePlayerSpawnData
    {
        void SetPosition(Vector2 position);

        void SetDirection(float direction);

        Vector2 GetPosition();

        float GetDirection();
    }
}