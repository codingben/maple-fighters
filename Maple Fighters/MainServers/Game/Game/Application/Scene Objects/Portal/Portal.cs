using MathematicsHelper;
using ServerApplication.Common.Components;
using Game.Common;
using InterestManagement;
using InterestManagement.Components;

namespace Game.Application.SceneObjects
{
    public class Portal : SceneObject
    {
        public Portal(Vector2 position, Maps destinationMap) 
            : base(IdGenerator.GetId(), "Portal", new TransformDetails(position, Vector2.Zero, Direction.Left))
        {
            Components.AddComponent(new PortalInfoProvider(destinationMap));
        }
    }
}