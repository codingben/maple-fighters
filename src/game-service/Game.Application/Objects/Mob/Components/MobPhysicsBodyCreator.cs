using Box2DX.Dynamics;
using Game.Application.Components;
using Game.Physics;

namespace Game.Application.Objects.Components
{
    public class MobPhysicsBodyCreator : ComponentBase
    {
        private IGameObject mob;
        private IPhysicsWorldManager physicsWorldManager;

        protected override void OnAwake()
        {
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            mob = gameObjectGetter.Get();

            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            presenceSceneProvider.SceneChanged += (gameScene) =>
            {
                var gamePhysicsCreator =
                    gameScene.Components.Get<IScenePhysicsCreator>();
                physicsWorldManager =
                    gamePhysicsCreator.GetPhysicsWorldManager();

                physicsWorldManager.AddBody(CreateBodyData());
            };
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();

            var id = mob.Id;

            physicsWorldManager?.RemoveBody(id);
        }

        private NewBodyData CreateBodyData()
        {
            var id = mob.Id;
            var x = mob.Transform.Position.X;
            var y = mob.Transform.Position.Y;
            var mobAttackPlayerHandler = Components.Get<IMobAttackPlayerHandler>();
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
            polygonDef.UserData = new MobContactEvents(mobAttackPlayerHandler);

            return new NewBodyData(id, bodyDef, polygonDef);
        }
    }
}