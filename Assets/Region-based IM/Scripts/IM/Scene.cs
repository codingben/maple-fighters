using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Gameplay;
using Scripts.UI;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class Scene : MonoBehaviour, IScene, ISceneEvents
    {
        public const string SCENE_TAG = "Scene";
        private const string REGION_GAME_OBJECT_NAME = "Region";

        public event Action RegionsCreated;
        public Vector2 RegionSize => regionSize;

        [Header("Interest Management")]
        [SerializeField] private Vector2 sceneSize;
        [SerializeField] private Vector2 regionSize;
        [Header("Region Visual Graphics")]
        [SerializeField] private bool showVisualGraphics = true;

        private IRegion[,] regions;
        private readonly HashSet<ISceneObject> sceneObjects = new HashSet<ISceneObject>();

        private void Awake()
        {
            StartCoroutine(CreateRegions());
        }

        private IEnumerator CreateRegions()
        {
            var regionsX = (int)(sceneSize.x / regionSize.x);
            var regionsY = (int)(sceneSize.y / regionSize.y);

            regions = new IRegion[regionsX, regionsY];

            var x = -(sceneSize.x / 2) + regionSize.x / 2;
            var y = -(sceneSize.y / 2) + regionSize.y / 2;

            var regionId = 1;
            var regionGameObject = Resources.Load<GameObject>(REGION_GAME_OBJECT_NAME);

            for (var i = 0; i < regionsX; i++)
            {
                for (var j = 0; j < regionsY; j++)
                {
                    var region = Instantiate(regionGameObject).GetComponent<Region>();
                    region.Id = regionId;
                    region.transform.position = new Vector3(x + (i * regionSize.x), y + (j * regionSize.y), region.transform.position.z);
                    region.PublisherArea = new Rectangle(position: new Vector2(x + (i * regionSize.x), y + (j * regionSize.y)),
                        size: new Vector3(regionSize.x, regionSize.y));
                    region.name.RemoveCloneFromName();

                    if (showVisualGraphics)
                    {
                        region.GetComponent<RegionVisualGraphics>()?.CreateRegionVisualGraphics();
                    }

                    regions[i, j] = region;

                    regionId++;
                    yield return new WaitForSeconds(0.1f);
                }
            }

            RegionsCreated?.Invoke();
        }

        public ISceneObject AddSceneObject(ISceneObject sceneObject)
        {
            if (!sceneObjects.Add(sceneObject))
            {
                Debug.LogWarning($"A scene object with a id #{sceneObject.Id} already exists in a scene.");
                return null;
            }

            Debug.Log($"A new scene object: {sceneObject.GameObject.name}");
            return sceneObject;
        }

        public void RemoveSceneObject(ISceneObject sceneObject)
        {
            if (!sceneObjects.Remove(sceneObject))
            {
                Debug.LogWarning($"A scene object with id #{sceneObject.Id} does not exist in a scene.");
                return;
            }

            Debug.Log($"Removed scene object: {sceneObject.GameObject.name}");

            RemoveSubscriptionFromPublishers(sceneObject);
        }

        private void RemoveSubscriptionFromPublishers(ISceneObject sceneObject)
        {
            foreach (var region in regions)
            {
                if (region == null)
                {
                    continue;
                }

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

            Debug.LogWarning($"Could not find a scene object with id #{id}");
            return null;
        }

        public IRegion[,] GetAllRegions() => regions;
    }
}