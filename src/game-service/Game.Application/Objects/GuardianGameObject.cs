using Common.MathematicsHelper;
using Game.Application.Objects.Components;
using InterestManagement;

namespace Game.Application.Objects
{
    public class GuardianGameObject : GameObject
    {
        public GuardianGameObject(int id, Vector2 position, IMatrixRegion<IGameObject> region)
            : base(id, name: "Guardian", region)
        {
            Transform.SetPosition(position);
        }

        public void AddBubbleNotification(string text, int time)
        {
            Components.Add(new BubbleNotificationSender(text, time));
        }
    }
}