using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.Entity.Components
{
    internal class Transform : IComponent
    {
        public Vector2 NewPosition { get; private set; }
        public Vector2 LastPosition { get; set; }

        public void SetPosition(Vector2 position)
        {
            NewPosition = position;
        }

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}