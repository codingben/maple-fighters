using Box2DX.Dynamics;
using Game.Application.Components;
using Game.Physics;

namespace Game.Application.Objects.Components
{
    public class MobPhysicsBodyCreator : ComponentBase
    {
        private IGameObject gameObject;

        protected override void OnAwake()
        {
            gameObject = Components.Get<IGameObjectGetter>().Get();

            var presenceMapProvider = Components.Get<IPresenceMapProvider>();
            presenceMapProvider.MapChanged += (gameScene) =>
            {
                gameScene.PhysicsWorldManager.AddBody(CreateBodyData());
            };
        }

        private NewBodyData CreateBodyData()
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(gameObject.Transform.Position.X, gameObject.Transform.Position.Y);

            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(0.3625f, 0.825f); // TODO: Get from config
            polygonDef.Density = 0.0f;// TODO: Get from config
            polygonDef.Filter = new FilterData()
            {
                GroupIndex = (short)LayerMask.Mob
            };
            polygonDef.UserData = new MobContactEvents(gameObject.Id);

            return new NewBodyData(gameObject.Id, bodyDef, polygonDef);
        }
    }
}