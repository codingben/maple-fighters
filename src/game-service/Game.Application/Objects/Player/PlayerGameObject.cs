using Box2DX.Dynamics;
using Common.ComponentModel;
using Common.MathematicsHelper;
using Game.Physics;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id, IComponent[] components)
            : base(id, name: "RemotePlayer", position: Vector2.Zero, size: new Vector2(10, 5), components)
        {
            // Left blank intentionally
        }

        public NewBodyData CreateBodyData()
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(Transform.Position.X, Transform.Position.Y);
            bodyDef.UserData = (IGameObject)this;

            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(0.3625f, 0.825f);
            polygonDef.Density = 0.1f;
            polygonDef.Filter = new FilterData()
            {
                GroupIndex = (short)LayerMask.Player
            };

            return new NewBodyData(Id, bodyDef, polygonDef);
        }
    }
}