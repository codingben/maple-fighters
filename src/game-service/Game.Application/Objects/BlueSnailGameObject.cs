using Common.MathematicsHelper;

namespace Game.Application.Objects
{
    public class BlueSnailGameObject : GameObject
    {
        public BlueSnailGameObject(int id, Vector2 position)
            : base(id, name: "BlueSnail")
        {
            Transform.SetPosition(position);
        }
    }
}