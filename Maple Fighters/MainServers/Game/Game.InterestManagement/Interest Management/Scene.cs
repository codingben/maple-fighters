using System.Collections.Generic;
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
        private readonly Dictionary<int, ISceneObject> sceneObjects = new Dictionary<int, ISceneObject>();

        protected Scene(Vector2 sceneSize, Vector2 regionSize)
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
            if (sceneObjects.ContainsKey(sceneObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{sceneObject.Id} already exists in a scene."), LogMessageType.Warning);
                return null;
            }

            var presenceSceneProvider = sceneObject.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            presenceSceneProvider.Scene = this;

            sceneObject.OnAwake();
            sceneObjects.Add(sceneObject.Id, sceneObject);

            var debug = (bool)Config.Global.Log.InterestManagement;
            if (debug)
            {
                LogUtils.Log(MessageBuilder.Trace($"A new scene object: {sceneObject.Name} Id: {sceneObject.Id}"));
            }
            return sceneObject;
        }

        public void RemoveSceneObject(int id)
        {
            if (!sceneObjects.ContainsKey(id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{id} does not exists in a scene."), LogMessageType.Warning);
                return;
            }

            var sceneObject = sceneObjects[id];
            sceneObject.OnDestroy();

            var presenceSceneProvider = sceneObject.Components?.GetComponent<IPresenceSceneProvider>()?.AssertNotNull();
            if (presenceSceneProvider != null)
            {
                presenceSceneProvider.Scene = null;
            }

            sceneObjects.Remove(id);

            var debug = (bool)Config.Global.Log.InterestManagement;
            if (debug)
            {
                var name = sceneObject.Name;
                var sceneObjectId = sceneObject.Id;
                LogUtils.Log(MessageBuilder.Trace($"Removed scene object: {name} Id: {sceneObjectId}"));
            }

            RemoveSceneObjectFromRegions(id);
        }

        /// <summary>
        /// Remove a scene objct for all other scene objects in his region.
        /// </summary>
        private void RemoveSceneObjectFromRegions(int id)
        {
            foreach (var region in regions)
            {
                if (region.HasSubscription(id))
                {
                    region.RemoveSubscriptionForAllSubscribers(id);
                }
            }
        }

        public void Dispose()
        {
            var scenesObjectsTemp = new List<ISceneObject>();
            scenesObjectsTemp.AddRange(sceneObjects.Values);

            foreach (var sceneObject in scenesObjectsTemp)
            {
                sceneObject.Dispose();
            }

            sceneObjects.Clear();

            Components?.Dispose();
        }

        public ISceneObject GetSceneObject(int id)
        {
            if (sceneObjects.TryGetValue(id, out var sceneObject))
            {
                return sceneObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id #{id}"), LogMessageType.Error);
            return null;
        }

        public IRegion[,] GetAllRegions()
        {
            return regions;
        }
    }
}