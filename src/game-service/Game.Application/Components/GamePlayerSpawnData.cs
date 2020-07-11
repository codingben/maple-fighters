using Common.MathematicsHelper;

namespace Game.Application.Components
{
    public class GamePlayerSpawnData : IGamePlayerSpawnData
    {
        private Vector2 position;

        public void SetSpawnPosition(Vector2 position)
        {
            this.position = position;
        }

        public Vector2 GetSpawnPosition()
        {
            return position;
        }
    }
}