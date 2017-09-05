using System;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.Entity.Components
{
    /*public class Transform : Component
    {
        public Vector2 Position { get; private set; }
        // public Vector2 NewPosition { get; set; }
        // public Vector2 LastPosition { get; set; }

        public event Action<Vector2> PositionChanged;

        public Transform(Vector2 startPosition) 
        {
            Position = startPosition;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;

            /*NewPosition = position;

            if (Vector2.Distance(NewPosition, LastPosition) < 0.1f)
            {
                return;
            }

            PositionChanged?.Invoke(position);

            // LastPosition = NewPosition;
        }
    }*/
}