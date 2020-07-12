using Box2DX.Dynamics;
using Common.MathematicsHelper;
using InterestManagement;
using Physics.Box2D;

namespace Game.Application.Objects
{
    public class BlueSnailGameObject : GameObject
    {
        public BlueSnailGameObject(int id, Vector2 position, IMatrixRegion<IGameObject> region)
            : base(id, name: "BlueSnail", region)
        {
            Transform.SetPosition(position);
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