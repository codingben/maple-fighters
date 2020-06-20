using Common.MathematicsHelper;
using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class GuardianGameObject : GameObject
    {
        public GuardianGameObject(
            int id,
            Vector2 position,
            IGameScene scene,
            string text,
            int time)
            : base(id, "Guardian")
        {
            Transform.SetPosition(position);
            Transform.SetSize(Vector2.One);

            Components.Add(new PresenceSceneProvider(scene));
            Components.Add(new ProximityChecker());
            Components.Add(new BubbleNotificationSender(text, time));
        }
    }
}