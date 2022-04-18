using InterestManagement;

namespace Game.Application.Components
{
    public interface IScenePlayerSpawnData
    {
        void SetPosition(Vector2 position);

        void SetSize(Vector2 size);

        void SetDirection(float direction);

        Vector2 GetPosition();

        Vector2 GetSize();

        float GetDirection();
    }
}