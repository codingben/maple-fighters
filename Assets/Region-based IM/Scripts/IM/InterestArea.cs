using System.Collections.Generic;
using System.Linq;
using Scripts.Gameplay;
using UnityEngine;

namespace InterestManagement.Scripts
{
    [RequireComponent(typeof(NearbySubscribers))]
    public class InterestArea : MonoBehaviour, IInterestArea
    {
        private IScene scene;
        private ISceneEvents sceneEvents;
        private ISceneObject sceneObject;

        private Rectangle interestArea;

        private void Awake()
        {
            sceneObject = GetComponent<ISceneObject>();

            var sceneGameObject = GameObject.FindGameObjectWithTag(Scene.SCENE_TAG);
            scene = sceneGameObject.GetComponent<IScene>();
            sceneEvents = sceneGameObject.GetComponent<ISceneEvents>();
            if (sceneEvents != null)
            {
                sceneEvents.RegionsCreated += OnRegionsCreated;
            }

            if (scene != null)
            {
                interestArea = new Rectangle(transform.position, scene.RegionSize);
            }
        }

        private void Start()
        {
            DetectOverlapsWithRegions();
        }

        private void OnDestroy()
        {
            if (sceneEvents != null)
            {
                sceneEvents.RegionsCreated -= OnRegionsCreated;
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