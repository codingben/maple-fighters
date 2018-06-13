using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;
using InterestManagement.Components.Interfaces;
using MathematicsHelper;
using Config = JsonConfig.Config;

namespace InterestManagement
{
    public class Scene : IScene
    {
        public IContainer Components { get; } = new Container();
        public Vector2 RegionSize { get; }

        private readonly IRegion[,] regions;
        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        public Scene(Vector2 sceneSize, Vector2 regionSize)
        {
            var regionsX = (int)(sceneSize.X / regionSize.X);
            var regionsY = (int)(sceneSize.Y / regionSize.Y);

            regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.X / 2) + regionSize.X / 2;
            var y = -(sceneSize.Y / 2) + regionSize.Y / 2;

            for (var i = 0; i < regionsX; i++)
            {
                for (var j = 0; j < regionsY; j++)
                {
                    var regionPositionX = x + (i * regionSize.X);
                    var regionPositionY = y + (j * regionSize.Y);
                    regions[i, j] = new Region(new Rectangle(position: new Vector2(regionPositionX, regionPositionY), size: new Vector2(regionSize.X, regionSize.Y)));
                }
            }

            RegionSize = regionSize;
        }

        public ISceneObject AddSceneObject(ISceneObject sceneObject)
        {
            if (!sceneObjects.Add(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{sceneObject.Id} already exists in a scene."), LogMessageType.Warning);
                return null;
            }

            Log();
            return sceneObject;

            void Log()
            {
                var debug = (bool)Config.Global.Log.InterestManagement;
                if (debug)
                {
                    LogUtils.Log(MessageBuilder.Trace($"A new scene object: {sceneObject.Name} Id: {sceneObject.Id}"));
                }
            }
        }

        public bool RemoveSceneObject(ISceneObject sceneObject)
        {
            if (!sceneObjects.Remove(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} does not exist in a scene."), LogMessageType.Warning);
                return false;
            }

            Log();
            RemoveSubscriptionFromPublishers();
            return true;

            void Log()
            {
                var debug = (bool)Config.Global.Log.InterestManagement;
                if (debug)
                {
                    var name = sceneObject.Name;
                    var sceneObjectId = sceneObject.Id;
                    LogUtils.Log(MessageBuilder.Trace($"Removed scene object: {name} Id: {sceneObjectId}"));
                }
            }

            void RemoveSubscriptionFromPublishers()
            {
                foreach (var region in regions)
                {
                    if (region.HasSubscription(sceneObject))
                    {
                        region.RemoveSubscriptionForAllSubscribers(sceneObject);
                    }
                }
            }
        }

        public void Dispose()
        {
            DisposeSceneObjects();

            Components?.Dispose();

            void DisposeSceneObjects()
            {
                var scenesObjectsTemp = new List<ISceneObject>();
                scenesObjectsTemp.AddRange(sceneObjects);

                foreach (var sceneObject in scenesObjectsTemp)
                {
                    sceneObject.Dispose();
                }

                sceneObjects.Clear();
            }
        }

        public ISceneObject GetSceneObject(int id)
        {
            var sceneObject = sceneObjects.SingleOrDefault(x => x.Id.Equals(id));
            if (sceneObject != null)
            {
                return sceneObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id #{id}"), LogMessageType.Warning);
            return null;
        }

        public IReadOnlyCollection<ISceneObject> GetAllSceneObjects() => sceneObjects;
        public IRegion[,] GetAllRegions() => regions;
    }
}