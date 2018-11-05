using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.InterestManagement
{
    public class InterestArea : IInterestArea
    {
        /// <inheritdoc />
        public event Action<IRegion[]> NearbyRegionsAdded;

        /// <inheritdoc />
        public event Action<IRegion[]> NearbyRegionsRemoved;

        private readonly IScene scene;
        private readonly ITransform transform;

        private readonly List<IRegion> nearbyRegions = new List<IRegion>();

        public InterestArea(IScene scene, ITransform transform)
        {
            this.scene = scene;
            this.transform = transform;
            
            SubscribeToTransformEvents();
        }

        public void Dispose()
        {
            UnsubscribeFromTransformEvents();
        }

        private void SubscribeToTransformEvents()
        {
            transform.PositionChanged += UpdateNearbyRegions;
        }

        private void UnsubscribeFromTransformEvents()
        {
            transform.PositionChanged -= UpdateNearbyRegions;
        }

        private void UpdateNearbyRegions()
        {
            SubscribeToNearbyRegions();
            UnsubscribeFromNearbyRegions();
        }

        private void SubscribeToNearbyRegions()
        {
            var newRegions = scene.MatrixRegion.GetRegions(transform).ToArray();

            foreach (var region in newRegions)
            {
                if (!nearbyRegions.Contains(region))
                {
                    nearbyRegions.Add(region);
                }
            }

            if (newRegions.Any())
            {
                NearbyRegionsAdded?.Invoke(newRegions);
            }
        }

        private void UnsubscribeFromNearbyRegions()
        {
            var oldRegions = 
                nearbyRegions.Where(
                    x => !x.Rectangle.Intersects(
                             transform.Position, 
                             transform.Size)).ToArray();

            foreach (var region in oldRegions)
            {
                nearbyRegions.Remove(region);
            }

            if (oldRegions.Any())
            {
                NearbyRegionsRemoved?.Invoke(oldRegions);
            }
        }
    }
}