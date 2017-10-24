using System.Collections.Generic;
using CommonTools.Log;
using Scripts.Gameplay;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class Scene : MonoBehaviour
    {
        public IRegion[,] Regions { get; private set; }
        public Vector2 RegionSize => regionSize;

        [SerializeField] private Vector2 sceneSize;
        [SerializeField] private Vector2 regionSize;
        [SerializeField] private GameObject regionGameObject;

        private readonly Dictionary<int, ISceneObject> sceneObjects = new Dictionary<int, ISceneObject>();

        private void Awake()
        {
            var regionsX = (int) (sceneSize.x / regionSize.x);
            var regionsY = (int) (sceneSize.y / regionSize.y);

            Regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.x / 2) + regionSize.x / 2;
            var y = -(sceneSize.y / 2) + regionSize.y / 2;

            var regionId = 1;

            for (var i = 0; i < Regions.GetLength(0); i++)
            {
                for (var j = 0; j < Regions.GetLength(1); j++)
                {
                    var region = Instantiate(regionGameObject).GetComponent<Region>();
                    region.Id = regionId;
                    region.name = $"I: {i} J: {j}";
                    region.transform.position = new Vector3(x + (i * regionSize.x), y + (j * regionSize.y));
                    region.transform.localScale = new Vector3(regionSize.x, regionSize.y);
                    region.PublisherArea = new Rectangle(new Vector2(x + (i * regionSize.x), y + (j * regionSize.y)),
                        new Vector3(regionSize.x, regionSize.y));

                    Regions[i, j] = region;

                    regionId++;
                }
            }
        }

        public ISceneObject AddSceneObject(ISceneObject sceneObject)
        {
            if (sceneObjects.ContainsKey(sceneObject.Id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{sceneObject.Id} already exists in a scene."), LogMessageType.Error);
                return null;
            }

            sceneObjects.Add(sceneObject.Id, sceneObject);

            LogUtils.Log(MessageBuilder.Trace($"A new scene object: {sceneObject.GetGameObject().name}"), LogMessageType.Error);
            return sceneObject;
        }

        public void RemoveSceneObject(int id)
        {
            if (!sceneObjects.ContainsKey(id))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{id} does not exists in a scene."), LogMessageType.Error);
                return;
            }

            LogUtils.Log(MessageBuilder.Trace($"Removed scene object: {sceneObjects[id].GetGameObject().name}"), LogMessageType.Error);

            sceneObjects.Remove(id);

            RemoveSubscriptionFromPublishers(id);
        }

        private void RemoveSubscriptionFromPublishers(int id)
        {
            foreach (var region in Regions)
            {
                if (region.HasSubscription(id))
                {
                    region.RemoveSubscriptionForAllSubscribers(id);
                }
            }
        }

        public ISceneObject GetSceneObject(int id)
        {
            ISceneObject sceneObject;
            if (sceneObjects.TryGetValue(id, out sceneObject))
            {
                return sceneObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id #{id}"), LogMessageType.Error);
            return null;
        }
    }
}