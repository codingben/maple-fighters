using Common.MathematicsHelper;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class GuardianGameObject : GameObject
    {
        public GuardianGameObject(int id, Vector2 position)
            : base(id, name: "Guardian")
        {
            Transform.SetPosition(position);
        }

        public void AddBubbleNotification(string text, int time)
        {
            Components.Add(new BubbleNotificationSender(text, time));
        }
    }
}