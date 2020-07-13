using Box2DX.Dynamics;
using Physics.Box2D;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id)
            : base(id, name: "Player")
        {
            // Left blank intentionally
        }

        public NewBodyData CreateBodyData()
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(Transform.Position.X, Transform.Position.Y);

            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(5, 5);
            polygonDef.Density = 0.0f;
            polygonDef.Filter = new FilterData();

            return new NewBodyData(Id, bodyDef, polygonDef);
        }
    }
}