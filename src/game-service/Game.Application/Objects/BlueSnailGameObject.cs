using Common.MathematicsHelper;
using InterestManagement;

namespace Game.Application.Objects
{
    public class BlueSnailGameObject : GameObject
    {
        public BlueSnailGameObject(int id, Vector2 position, IMatrixRegion<IGameObject> region)
            : base(id, name: "BlueSnail", region)
        {
            Transform.SetPosition(position);
        }
    }
}