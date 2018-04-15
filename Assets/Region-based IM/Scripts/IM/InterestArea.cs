using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class InterestArea : MonoBehaviour, IInterestArea
    {
        [SerializeField] private Transform interestAreaTransform;

        private IScene scene;
        private ISceneObject sceneObject;
        private Rectangle interestArea;

        private void Awake()
        {
            var sceneEvents = GameObject.FindGameObjectWithTag("Scene").GetComponent<ISceneEvents>();
            sceneEvents.RegionsCreated += OnRegionsCreated;

            scene = GameObject.FindGameObjectWithTag("Scene").GetComponent<IScene>();
            interestArea = new Rectangle(transform.position, scene.RegionSize);

            sceneObject = GetComponent<ISceneObject>();
            gameObject.AddComponent(typeof(NearbySubscribers));

            if (interestAreaTransform != null)
            {
                interestAreaTransform.localScale = new Vector3(scene.RegionSize.x, scene.RegionSize.y, interestAreaTransform.localScale.z);
            }
        }

        private void OnRegionsCreated()
        {
            DetectOverlapsWithRegions();
        }

        private void Update()
        {
            SetPosition(transform.position);
        }

        private void SetPosition(Vector2 position)
        {
            interestArea.SetPosition(position);

            if (scene != null)
            {
                DetectOverlapsWithRegions();
            }
        }

        private void DetectOverlapsWithRegions()
        {
            var sceneRegions = scene.GetAllRegions();

            foreach (var region in sceneRegions)
            {
                if (region == null)
                {
                    continue;
                }

                if (IsIntersect(region.PublisherArea, interestArea))
                {
                    if (region.HasSubscription(sceneObject))
                    {
                        continue;
                    }

                    region.AddSubscription(sceneObject);
                }
                else
                {
                    if (region.HasSubscription(sceneObject))
                    {
                        region.RemoveSubscription(sceneObject);
                    }
                }
            }
        }

        private bool IsIntersect(Rectangle publisherArea, Rectangle interestArea) => !Rectangle.Intersect(publisherArea, interestArea).Equals(Rectangle.Empty);

        public IEnumerable<IRegion> GetSubscribedPublishers()
        {
            var regions = scene.GetAllRegions();
            return regions.Cast<IRegion>().Where(region => region.HasSubscription(sceneObject)).ToArray();
        }
    }
}