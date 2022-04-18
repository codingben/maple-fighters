using InterestManagement;

namespace Game.Application.Components
{
    public class ScenePlayerSpawnData : ComponentBase, IScenePlayerSpawnData
    {
        public Vector2Data Position { get; set; }

        public Vector2Data Size { get; set; }

        public float Direction { get; set; }

        public void SetPosition(Vector2 position)
        {
            Position = new Vector2Data()
            {
                X = position.X,
                Y = position.Y
            };
        }

        public void SetSize(Vector2 size)
        {
            Size = new Vector2Data()
            {
                X = size.X,
                Y = size.Y
            };
        }

        public void SetDirection(float direction)
        {
            Direction = direction;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(Position.X, Position.Y);
        }

        public Vector2 GetSize()
        {
            return new Vector2(Size.X, Size.Y);
        }

        public float GetDirection()
        {
            return Direction;
        }
    }
}