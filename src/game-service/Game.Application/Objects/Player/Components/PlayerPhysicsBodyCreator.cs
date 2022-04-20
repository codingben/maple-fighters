using Box2DX.Dynamics;
using Game.Application.Components;
using Game.Physics;

namespace Game.Application.Objects.Components
{
    public class PlayerPhysicsBodyCreator : ComponentBase
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
            var bodyDef = new BodyDef();
            var x = gameObject.Transform.Position.X;
            var y = gameObject.Transform.Position.Y;
            bodyDef.Position.Set(x, y);
            bodyDef.UserData = gameObject;

            var playerConfigDataProvider =
                Components.Get<IPlayerConfigDataProvider>();
            var playerConfigData = playerConfigDataProvider.Provide();
            var bodyWidth = playerConfigData.BodyWidth;
            var bodyHeight = playerConfigData.BodyHeight;
            var bodyDensity = playerConfigData.BodyDensity;
            var polygonDef = new PolygonDef();
            polygonDef.SetAsBox(bodyWidth, bodyHeight);
            polygonDef.Density = bodyDensity;
            polygonDef.Filter = new FilterData()
            {
                GroupIndex = (short)LayerMask.Player
            };

            return new NewBodyData(id, bodyDef, polygonDef);
        }
    }
}