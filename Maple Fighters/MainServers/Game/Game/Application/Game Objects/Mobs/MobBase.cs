using InterestManagement;
using MathematicsHelper;
using Physics.Box2D.Core;

namespace Game.Application.GameObjects
{
    public class MobBase : BodyGameObject
    {
        protected MobBase(string name, Vector2 position, Vector2 size) 
            : base(name, new TransformDetails(position, size, Direction.Left), LayerMask.Mob)
        {
            // Left blank intentionally
        }
    }
}