using Common.MathematicsHelper;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id, Vector2 position)
            : base(id, name: "Player")
        {
            Transform.SetPosition(position);
        }
    }
}