using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.Entity.Components
{
    internal class Transform : IComponent, ITransform
    {
        public Vector2 Position { get; private set; }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public Vector2 GetPosition()
        {
            return Position;
        }

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}