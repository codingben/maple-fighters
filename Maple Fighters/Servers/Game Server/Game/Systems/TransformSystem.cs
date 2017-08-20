using Game.Entities;
using ServerApplication.Common.ComponentModel;

namespace Game.Systems
{
    internal class TransformSystem : IComponent
    {
        private readonly EntityContainer entityContainer;

        public TransformSystem()
        {
            entityContainer = ServerComponents.Container.GetComponent<EntityContainer>() as EntityContainer;
        }

        public void Dispose()
        {
            entityContainer?.Dispose();
        }
    }
}