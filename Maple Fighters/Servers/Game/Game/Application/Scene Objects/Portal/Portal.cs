using Game.InterestManagement;
using MathematicsHelper;
using Shared.Game.Common;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.SceneObjects
{
    public class Portal : SceneObject
    {
        public Portal(Vector2 position, Maps destinationMap) 
            : base("Portal", position, Direction.Left)
        {
            Container.AddComponent(new PortalInfoProvider(destinationMap));
        }
    }
}