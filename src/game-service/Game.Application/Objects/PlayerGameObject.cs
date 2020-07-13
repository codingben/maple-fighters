using Box2DX.Dynamics;
using Game.Application.Objects.Components;
using Physics.Box2D;

namespace Game.Application.Objects
{
    public class PlayerGameObject : GameObject
    {
        public PlayerGameObject(int id)
            : base(id, name: "Player")
        {
            AddCommonComponents();
        }

        private void AddCommonComponents()
        {
            Components.Add(new AnimationData());
            Components.Add(new CharacterData());
            Components.Add(new PresenceMapProvider());
        }

        public NewBodyData CreateBodyData()
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(Transform.Position.X, Transform.Position.Y);
            bodyDef.UserData = (IGameObject)this;

            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(5, 5);
            polygonDef.Density = 0.1f;
            polygonDef.Filter = new FilterData()
            {
                GroupIndex = (short)LayerMask.Player
            };

            return new NewBodyData(Id, bodyDef, polygonDef);
        }
    }
}