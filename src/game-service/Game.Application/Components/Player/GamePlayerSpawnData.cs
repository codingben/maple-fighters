using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public class GamePlayerSpawnData : IGamePlayerSpawnData
    {
        private Vector2 position;
        private float direction;

        public void SetSpawnPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetSpawnDirection(float direction)
        {
            this.direction = direction;
        }

        public Vector2 GetSpawnPosition()
        {
            return position;
        }

        public float GetSpawnDirection()
        {
            return direction;
        }
    }
}