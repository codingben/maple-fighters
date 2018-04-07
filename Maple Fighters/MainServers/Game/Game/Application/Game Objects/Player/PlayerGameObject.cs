using InterestManagement;
using Physics.Box2D.Core;

namespace Game.Application.GameObjects
{
    internal class PlayerGameObject : BodyGameObject
    {
        public PlayerGameObject(TransformDetails transformDetails) 
            : base("Player", transformDetails, LayerMask.Player)
        {
            // Left blank intentionally
        }
    }
}