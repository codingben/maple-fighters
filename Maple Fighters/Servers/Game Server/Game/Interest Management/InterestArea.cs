using System.Collections.Generic;
using CommonTools.Coroutines;
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

        private readonly ICoroutinesExecuter coroutinesExecuter;

        public InterestArea(IEntity entity, Vector2 areaSize, ICoroutinesExecuter coroutinesExecuter) 
            : base(entity)
        {
            this.coroutinesExecuter = coroutinesExecuter;

            areaBoundaries = new Rectangle(Vector2.Zero, areaSize);

            var transform = OwnerEntity.Components.GetComponent<Transform>().AssertNotNull() as Transform;
            transform.PositionChanged += SetPosition;

            SetSceneRegions();
        }

        private void SetSceneRegions()
        {
            var sceneContainer = ServerComponents.Container.GetComponent<SceneContainer>().AssertNotNull() as SceneContainer;
            var scene = sceneContainer.GetScene(OwnerEntity.PresenceSceneId).AssertNotNull();
            if (scene == null)
            {
                return;
            }

            sceneRegions = scene.GetAllRegions();

            coroutinesExecuter.StartCoroutine(DetectOverlapsWithRegions());
        }

        public void SetPosition(Vector2 position)
        {
            areaBoundaries.SetPosition(position);
        }

        public IRegion[,] GetPublishers()
        {
            return sceneRegions;
        }

        private IEnumerator<IYieldInstruction> DetectOverlapsWithRegions()
        {
            var lastPosition = Vector2.Zero;

            while (true)
            {
                if (Vector2.Distance(areaBoundaries.Position, lastPosition) < 1)
                {
                    yield return null;
                }

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

                lastPosition = areaBoundaries.Position;
                yield return null;
            }
        }
    }
}