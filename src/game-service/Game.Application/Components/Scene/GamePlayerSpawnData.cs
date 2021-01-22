using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public class GamePlayerSpawnData : IGamePlayerSpawnData
    {
        private Vector2 position;
        private float direction;

        public void SetPosition(Vector2 position)
        {
            this.position = position;
        }

        public void SetDirection(float direction)
        {
            this.direction = direction;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public float GetDirection()
        {
            return direction;
        }
    }
}