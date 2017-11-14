using System.Globalization;
using System.IO;
using Box2DX.Collision;
using Box2DX.Dynamics;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using MathematicsHelper;
using ServiceStack;
using Shared.Game.Common;

namespace Physics.Box2D
{
    public class PhysicsWorldCreator : Component<ISceneEntity>
    {
        private readonly World world;

        public PhysicsWorldCreator(Maps map, PhysicsWorldInfo worldInfo)
        {
            var worldAabb = new AABB
            {
                LowerBound = worldInfo.LowerBound.FromVector2(),
                UpperBound = worldInfo.UpperBound.FromVector2()
            };
            world = new World(worldAabb, worldInfo.Gravity.FromVector2(), worldInfo.DoSleep);
            world.SetContactFilter(new ContactFilterModified());

            CreateScenePhysicsData(map);
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            Entity.Container.AddComponent(new PhysicsWorldProvider(world));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            world.Dispose();
        }

        private void CreateScenePhysicsData(Maps map)
        {
            var path = $"python/scenes/{map}/ScenePhysicsData.json";
            if (!File.Exists(path))
            {
                LogUtils.Log($"Could not find ScenePhysicsData json file for {map}.");
                return;
            }

            var json = File.ReadAllText(path);
            var scenePhysicsData = DynamicJson.Deserialize(json);

            foreach (var groundCollider in scenePhysicsData.GroundColliders)
            {
                world.CreateGround(
                    new Vector2(
                        float.Parse(groundCollider.Position.X, CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(groundCollider.Position.Y, CultureInfo.InvariantCulture.NumberFormat)),
                    new Vector2(
                        float.Parse(groundCollider.Extents.X, CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(groundCollider.Extents.Y, CultureInfo.InvariantCulture.NumberFormat))
                );
            }
        }
    }
}