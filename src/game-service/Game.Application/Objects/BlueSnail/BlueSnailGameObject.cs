using Box2DX.Dynamics;
using Common.ComponentModel;
using Common.MathematicsHelper;
using Physics.Box2D;

namespace Game.Application.Objects
{
    public class BlueSnailGameObject : GameObject
    {
        public BlueSnailGameObject(int id, Vector2 position, IComponent[] components)
            : base(id, name: "BlueSnail", components)
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
            polygonDef.Filter = new FilterData()
            {
                GroupIndex = (short)LayerMask.Mob
            };
            polygonDef.UserData = new MobContactEvents(Id);

            return new NewBodyData(Id, bodyDef, polygonDef);
        }
    }
}