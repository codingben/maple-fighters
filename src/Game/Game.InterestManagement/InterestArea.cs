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

            SubscribeToPositionChanged();
        }

        public void Dispose()
        {
            UnsubscribeFromPositionChanged();
        }

        private void SubscribeToPositionChanged()
        {
            transform.PositionChanged += UpdateNearbyRegions;
        }

        private void UnsubscribeFromPositionChanged()
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
            var newRegions =
                scene.MatrixRegion.GetRegions(transform)?.ToArray();

            if (newRegions != null)
            {
                foreach (var region in newRegions)
                {
                    if (!nearbyRegions.Contains(region))
                    {
                        nearbyRegions.Add(region);
                    }
                }

                if (newRegions.Length != 0)
                {
                    NearbyRegionsAdded?.Invoke(newRegions);
                }
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

            if (oldRegions.Length != 0)
            {
                NearbyRegionsRemoved?.Invoke(oldRegions);
            }
        }
    }
}