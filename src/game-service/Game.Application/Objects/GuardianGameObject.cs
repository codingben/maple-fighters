using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class GuardianGameObject : GameObject
    {
        public GuardianGameObject(int id)
            : base(id, name: "Guardian")
        {
            Components.Add(new GameObjectGetter(this));
        }

        public void AddProximityChecker(IGameScene gameScene)
        {
            Components.Add(new ProximityChecker(gameScene));
        }

        public void AddBubbleNotification(string text, int time)
        {
            Components.Add(new BubbleNotificationSender(text, time));
        }
    }
}