using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Scripts.Gameplay;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class Scene : MonoBehaviour
    {
        public Vector2 RegionSize => regionSize;

        [SerializeField] private Vector2 sceneSize;
        [SerializeField] private Vector2 regionSize;
        [SerializeField] private GameObject regionGameObject;

        private IRegion[,] regions;
        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        private void Awake()
        {
            CreateRegions();
        }

        private void CreateRegions()
        {
            var regionsX = (int)(sceneSize.x / regionSize.x);
            var regionsY = (int)(sceneSize.y / regionSize.y);

            regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.x / 2) + regionSize.x / 2;
            var y = -(sceneSize.y / 2) + regionSize.y / 2;

            var regionId = 1;

            for (var i = 0; i < regionsX; i++)
            {
                for (var j = 0; j < regionsY; j++)
                {
                    var region = Instantiate(regionGameObject.AssertNotNull("Could not find region game object.")).GetComponent<Region>();
                    region.Id = regionId;
                    region.transform.position = new Vector3(x + (i * regionSize.x), y + (j * regionSize.y), -1);
                    region.transform.localScale = new Vector3(regionSize.x, regionSize.y);
                    region.PublisherArea = new Rectangle(position: new Vector2(x + (i * regionSize.x), y + (j * regionSize.y)),
                        size: new Vector3(regionSize.x, regionSize.y));

                    regions[i, j] = region;

                    regionId++;
                }
            }
        }

        public ISceneObject AddSceneObject(ISceneObject sceneObject)
        {
            if (!sceneObjects.Add(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with a id #{sceneObject.Id} already exists in a scene."), LogMessageType.Warning);
                return null;
            }

            LogUtils.Log(MessageBuilder.Trace($"A new scene object: {sceneObject.GetGameObject().name}"));
            return sceneObject;
        }

        public void RemoveSceneObject(ISceneObject sceneObject)
        {
            if (!sceneObjects.Remove(sceneObject))
            {
                LogUtils.Log(MessageBuilder.Trace($"A scene object with id #{sceneObject.Id} does not exist in a scene."), LogMessageType.Warning);
                return;
            }

            LogUtils.Log(MessageBuilder.Trace($"Removed scene object: {sceneObject.GetGameObject().name}"));

            RemoveSubscriptionFromPublishers(sceneObject);
        }

        private void RemoveSubscriptionFromPublishers(ISceneObject sceneObject)
        {
            foreach (var region in regions)
            {
                if (region.HasSubscription(sceneObject))
                {
                    region.RemoveSubscriptionForAllSubscribers(sceneObject);
                }
            }
        }

        public ISceneObject GetSceneObject(int id)
        {
            var sceneObject = sceneObjects.Single(x => x.Id.Equals(id));
            if (sceneObject != null)
            {
                return sceneObject;
            }

            LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id #{id}"), LogMessageType.Warning);
            return null;
        }

        public IRegion[,] GetAllRegions() => regions;
    }
}