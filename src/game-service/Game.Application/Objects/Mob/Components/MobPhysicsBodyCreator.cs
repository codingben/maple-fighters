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
                var gamePhysicsCreator =
                    gameScene.Components.Get<IScenePhysicsCreator>();
                var physicsWorldManager =
                    gamePhysicsCreator.GetPhysicsWorldManager();

                physicsWorldManager.AddBody(CreateBodyData());
            };
        }

        private NewBodyData CreateBodyData()
        {
            var id = gameObject.Id;
            var x = gameObject.Transform.Position.X;
            var y = gameObject.Transform.Position.Y;
            var mobConfigDataProvider = Components.Get<IMobConfigDataProvider>();
            var mobConfigData = mobConfigDataProvider.Provide();
            var bodyWidth = mobConfigData.BodyWidth;
            var bodyHeight = mobConfigData.BodyHeight;
            var bodyDensity = mobConfigData.BodyDensity;
            var bodyDef = new BodyDef();
            var polygonDef = new PolygonDef();

            bodyDef.Position.Set(x, y);

            polygonDef.SetAsBox(bodyWidth, bodyHeight);
            polygonDef.Density = bodyDensity;
            polygonDef.Filter = new FilterData()
            {
                GroupIndex = (short)LayerMask.Mob
            };
            polygonDef.UserData = new MobContactEvents(id);

            return new NewBodyData(id, bodyDef, polygonDef);
        }
    }
}