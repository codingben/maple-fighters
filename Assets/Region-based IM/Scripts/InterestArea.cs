using System;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Scripts.Gameplay;
using UnityEngine;

namespace InterestManagement.Scripts
{
    public class InterestArea : MonoBehaviour, IInterestArea
    {
        private Scene scene;
        private NetworkIdentity networkIdentity;
        private Rectangle interestArea;
        private static int id;

        private void Awake()
        {
            scene = FindObjectOfType<Scene>().AssertNotNull();
            interestArea = new Rectangle(transform.position, scene.RegionSize);

            networkIdentity = GetComponent<NetworkIdentity>().AssertNotNull();
            networkIdentity.Id = ++id;

            name = $"Entity {networkIdentity.Id}";

            gameObject.AddComponent(typeof(NearbySubscribers));
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

        public void SetSize()
        {
            throw new NotImplementedException();
        }

        public void DetectOverlapsWithRegions()
        {
            var sceneRegions = scene.GetAllRegions();
            var sceneObject = networkIdentity;

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
            var sceneObject = networkIdentity;
            return regions.Cast<IRegion>().Where(region => region.HasSubscription(sceneObject)).ToArray();
        }
    }
}