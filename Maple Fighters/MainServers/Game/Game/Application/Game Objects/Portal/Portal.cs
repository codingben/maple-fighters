using MathematicsHelper;
using Game.Common;
using InterestManagement;

namespace Game.Application.GameObjects
{
    public class Portal : GameObject
    {
        public Portal(Vector2 position, Maps destinationMap) 
            : base("Portal", new TransformDetails(position, Vector2.Zero, Direction.Left))
        {
            Components.AddComponent(new PortalInfoProvider(destinationMap));
        }
    }
}