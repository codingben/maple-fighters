using Box2DX.Dynamics;
using Game.Application.Objects.Components;
using Game.Physics;

namespace Game.Application.Objects
{
    public class BlueSnailContactEvents : IContactEvents
    {
        private readonly int id;

        public BlueSnailContactEvents(int id)
        {
            this.id = id;
        }

        public void OnBeginContact(Body body)
        {
            var player = body.GetUserData() as IGameObject;
            var playerAttacked = player?.Components.Get<IPlayerAttacked>();
            playerAttacked?.Attack(id);
        }

        public void OnEndContact(Body body)
        {
            // Left blank intentionally
        }
    }
}