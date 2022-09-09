using Box2DX.Dynamics;
using Game.Application.Components;
using Game.Physics;

namespace Game.Application.Objects.Components
{
    public class PlayerPhysicsBodyCreator : ComponentBase
    {
        private IGameObject player;

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            player = gameObjectGetter.Get();

            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            presenceSceneProvider.SceneChanged += (gameScene) =>
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
            var id = player.Id;
            var x = player.Transform.Position.X;
            var y = player.Transform.Position.Y;
            var playerConfigDataProvider = Components.Get<IPlayerConfigDataProvider>();
            var playerConfigData = playerConfigDataProvider.Provide();
            var bodyWidth = playerConfigData.BodyWidth;
            var bodyHeight = playerConfigData.BodyHeight;
            var bodyDensity = playerConfigData.BodyDensity;
            var bodyDef = new BodyDef();
            var polygonDef = new PolygonDef();

            bodyDef.Position.Set(x, y);
            bodyDef.UserData = player;

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