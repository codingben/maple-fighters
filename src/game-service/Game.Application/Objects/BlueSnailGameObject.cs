using Common.MathematicsHelper;
using Game.Application.Components;
using Game.Application.Objects.Components;

namespace Game.Application.Objects
{
    public class BlueSnailGameObject : GameObject
    {
        public BlueSnailGameObject(int id, Vector2 position, IGameScene scene)
            : base(id, "BlueSnail")
        {
            Transform.SetPosition(position);
            Transform.SetSize(Vector2.One);

            Components.Add(new PresenceSceneProvider(scene));
            Components.Add(new ProximityChecker());
        }
    }
}