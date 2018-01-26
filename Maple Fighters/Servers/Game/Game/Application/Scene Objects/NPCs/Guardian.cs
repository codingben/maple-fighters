using Game.InterestManagement;
using MathematicsHelper;
using ServerApplication.Common.Components;

namespace Game.Application.SceneObjects
{
    public class Guardian : SceneObject
    {
        public Guardian(string name, Vector2 position, Direction direction) 
            : base(IdGenerator.GetId(), name, new TransformDetails(position, Vector2.Zero, direction))
        {
            // Left blank intentionally
        }
    }
}