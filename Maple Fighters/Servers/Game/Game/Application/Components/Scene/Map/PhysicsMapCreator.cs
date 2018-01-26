using System.Globalization;
using System.IO;
using CommonTools.Log;
using ComponentModel.Common;
using MathematicsHelper;
using Physics.Box2D;
using ServiceStack;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class PhysicsMapCreator : Component
    {
        private readonly Maps map;

        public PhysicsMapCreator(Maps map)
        {
            this.map = map;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            CreateScenePhysics(map);
        }

        private void CreateScenePhysics(Maps map)
        {
            var physicsWorldProvider = Entity.GetComponent<IPhysicsWorldProvider>().AssertNotNull();

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
                physicsWorldProvider.GetWorld().CreateGround(
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