using CommonTools.Log;
using Game.Entities;
using Game.Entity.Components;
using MathematicsHelper;
using ServerApplication.Common.ComponentModel;

namespace Game.InterestManagement
{
    internal class InterestArea : EntityComponent, IInterestArea
    {
        private Rectangle areaBoundaries;
        private IRegion[,] sceneRegions;

        public InterestArea(IEntity entity, Vector2 areaSize) 
            : base(entity)
        {
            areaBoundaries = new Rectangle(Vector2.Zero, areaSize);

            var transform = OwnerEntity.Components.GetComponent<Transform>().AssertNotNull();
            transform.PositionChanged += SetPosition;

            SetSceneRegions();
        }

        private void SetSceneRegions()
        {
            var sceneContainer = ServerComponents.Container.GetComponent<SceneContainer>().AssertNotNull();
            var scene = sceneContainer.GetScene(OwnerEntity.PresenceSceneId).AssertNotNull();
            if (scene == null)
            {
                return;
            }

            sceneRegions = scene.GetAllRegions();
        }

        public void SetPosition(Vector2 position)
        {
            areaBoundaries.SetPosition(position);

            DetectOverlapsWithRegions();
        }

        public IRegion[,] GetPublishers()
        {
            return sceneRegions;
        }

        private void DetectOverlapsWithRegions()
        {
            foreach (var region in sceneRegions)
            {
                if (!Rectangle.Intersect(region.Rectangle, areaBoundaries).Equals(Rectangle.EMPTY))
                {
                    if (region.HasSubscription(OwnerEntity.Id))
                    {
                        continue;
                    }

                    region.AddSubscription(OwnerEntity);
                }
                else
                {
                    if (region.HasSubscription(OwnerEntity.Id))
                    {
                        region.RemoveSubscription(OwnerEntity);
                    }
                }
            }
        }
    }
}