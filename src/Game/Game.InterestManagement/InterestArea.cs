using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class InterestArea : IInterestArea
    {
        /// <inheritdoc />
        public event Action<ISceneObject> NearbySceneObjectAdded;

        /// <inheritdoc />
        public event Action<ISceneObject> NearbySceneObjectRemoved;

        private readonly ISceneObject sceneObject;

        private readonly HashSet<IRegion> nearbyRegions = new HashSet<IRegion>();
        private readonly HashSet<ISceneObject> nearbySceneObjects = new HashSet<ISceneObject>();

        public InterestArea(ISceneObject sceneObject)
        {
            this.sceneObject = sceneObject;
            
            SubscribeToTransformEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromTransformEvents();
        }

        private void UpdateNearbyRegions()
        {
            SubscribeToNearbyRegionsIfNeeded();

            var regions = nearbyRegions.ToArray();
            foreach (var region in regions)
            {
                var otherRectangle = sceneObject.Transform.Rectangle;
                if (region.GetRectangle().Intersects(otherRectangle))
                {
                    if (region.Subscribe(sceneObject))
                    {
                        // TODO: Optimize if possible
                        foreach (var sceneObject in region.GetAllSceneObjects())
                        {
                            if (nearbySceneObjects.Add(sceneObject))
                            {
                                NearbySceneObjectAdded?.Invoke(sceneObject);
                            }
                        }
                    }
                }
                else
                {
                    if (region.Unsubscribe(sceneObject))
                    {
                        nearbyRegions.Remove(region);

                        // TODO: Optimize if possible
                        foreach (var sceneObject in region.GetAllSceneObjects())
                        {
                            if (nearbySceneObjects.Remove(sceneObject))
                            {
                                NearbySceneObjectRemoved?.Invoke(sceneObject);
                            }
                        }
                    }
                }
            }
        }

        private void SubscribeToNearbyRegionsIfNeeded()
        {
            var corners = sceneObject.Transform.Rectangle.GetFixedCorners();
            var regions = sceneObject.Scene.MatrixRegion.GetRegions(corners);

            foreach (var region in regions)
            {
                nearbyRegions.Add(region);
            }
        }

        private void SubscribeToTransformEvents()
        {
            sceneObject.Transform.PositionChanged += UpdateNearbyRegions;
            sceneObject.Transform.SizeChanged += UpdateNearbyRegions;
        }

        private void UnsubscribeFromTransformEvents()
        {
            sceneObject.Transform.PositionChanged -= UpdateNearbyRegions;
            sceneObject.Transform.SizeChanged -= UpdateNearbyRegions;
        }
    }
}