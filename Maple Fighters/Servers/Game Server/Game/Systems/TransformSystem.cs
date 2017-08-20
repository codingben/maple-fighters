using System.Collections.Generic;
using CommonTools.Log;
using Game.Entities;
using Game.Entity.Components;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.Systems
{
    internal class TransformSystem : IComponent
    {
        public void UpdatePosition(IEnumerable<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                var transform = entity.Components.GetComponent<Transform>().AssertNotNull() as Transform;

                if (Vector2.Distance(transform.NewPosition, transform.LastPosition) > 1)
                {
                    // Broadcast event...
                }

                transform.LastPosition = transform.NewPosition;
            }
        }

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}