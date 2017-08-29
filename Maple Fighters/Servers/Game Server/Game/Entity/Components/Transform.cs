using System;
using Game.Entities;
using MathematicsHelper;

namespace Game.Entity.Components
{
    internal class Transform : EntityComponent
    {
        public Vector2 NewPosition { get; private set; }
        public Vector2 LastPosition { get; set; }

        public event Action<Vector2> PositionChanged;

        public Transform(IEntity entity) 
            : base(entity)
        {
            // Left blank intentionally
        }

        public void SetPosition(Vector2 position)
        {
            NewPosition = position;

            PositionChanged?.Invoke(position);
        }
    }
}