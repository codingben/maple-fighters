using System.Collections.Generic;
using CommonTools.Log;
using MathematicsHelper;

namespace Game.InterestManagement
{
    public class Scene : IScene
    {
        public Vector2 RegionSize { get; }

        private readonly IRegion[,] regions;
        private readonly Dictionary<int, ISceneObject> sceneObjects = new Dictionary<int, ISceneObject>();

        private readonly object locker = new object();

        public Scene(Vector2 sceneSize, Vector2 regionSize)
        {
            var regionsX = (int)(sceneSize.X / regionSize.X);
            var regionsY = (int)(sceneSize.Y / regionSize.Y);

            regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.X / 2) + regionSize.X / 2;
            var y = -(sceneSize.Y / 2) + regionSize.Y / 2;

            for (var i = 0; i < regions.GetLength(0); i++)
            {
                for (var j = 0; j < regions.GetLength(1); j++)
                {
                    var regionPositionX = x + (i * regionSize.X);
                    var regionPositionY = y + (j * regionSize.Y);

                    regions[i, j] = new Region(new Rectangle(new Vector2(regionPositionX, regionPositionY), new Vector2(regionSize.X, regionSize.Y)));
                }
            }

            RegionSize = regionSize;
        }

        public ISceneObject AddSceneObject(ISceneObject sceneObject)
        {
            lock (locker)
            {
                if (sceneObjects.ContainsKey(sceneObject.Id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{sceneObject.Id} already exists in a scene."), LogMessageType.Warning);
                    return null;
                }

                sceneObject.Scene = this;
                sceneObjects.Add(sceneObject.Id, sceneObject);

                LogUtils.Log(MessageBuilder.Trace($"A new scene object: {sceneObject.Name}"));
                return sceneObject;
            }
        }

        public void RemoveSceneObject(int id)
        {
            lock (locker)
            {
                if (!sceneObjects.ContainsKey(id))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{id} does not exists in a scene."), LogMessageType.Warning);
                    return;
                }

                LogUtils.Log(MessageBuilder.Trace($"Removed scene object: {sceneObjects[id].Name}"));

                sceneObjects[id].Scene = null;
                sceneObjects.Remove(id);

                RemoveSubscriptionFromPublishers(id);
            }
        }

        private void RemoveSubscriptionFromPublishers(int id)
        {
            foreach (var region in regions)
            {
                if (region.HasSubscription(id))
                {
                    region.RemoveSubscriptionForAllSubscribers(id);
                }
            }
        }

        public ISceneObject GetSceneObject(int id)
        {
            lock (locker)
            {
                if (sceneObjects.TryGetValue(id, out var sceneObject))
                {
                    return sceneObject;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id #{id}"), LogMessageType.Error);
                return null;
            }
        }

        public IRegion[,] GetAllRegions()
        {
            return regions;
        }
    }
}