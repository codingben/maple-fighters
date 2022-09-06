using Box2DX.Dynamics;
using Game.Application.Objects.Components;
using Game.Physics;

namespace Game.Application.Objects
{
    public class MobContactEvents : IContactEvents
    {
        private readonly IMobAttackPlayerHandler attackPlayerHandler;

        public MobContactEvents(IMobAttackPlayerHandler attackPlayerHandler)
        {
            this.attackPlayerHandler = attackPlayerHandler;
        }

        public void OnBeginContact(Body body)
        {
            if (body.GetUserData() is IGameObject player)
            {
                attackPlayerHandler.Attack(player);
            }
        }

        public void OnEndContact(Body body)
        {
            // Left blank intentionally
        }
    }
}