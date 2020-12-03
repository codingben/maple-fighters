using InterestManagement;
using MathematicsHelper;

namespace Game.Application.GameObjects
{
    public class Guardian : GameObject
    {
        public Guardian(string name, Vector2 position, Direction direction) 
            : base(name, new TransformDetails(position, Vector2.Zero, direction))
        {
            // Left blank intentionally
        }

        public void CreateBubbleMessageNotifier(string message, int time)
        {
            Components.AddComponent(new BubbleMessageNotifier(message, time));
        }
    }
}