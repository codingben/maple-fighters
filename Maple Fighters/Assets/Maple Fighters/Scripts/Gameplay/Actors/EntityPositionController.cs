using Scripts.Containers;
using Scripts.Containers.Entity;
using Scripts.Containers.Service;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class EntityPositionController : MonoBehaviour
    {
        private IEntityContainer entityContainer;

        private void Start()
        {
            entityContainer = GameContainers.EntityContainer;

            ServiceContainer.GameService.EntityPositionChanged.AddListener(EntityPositionChanged);
            ServiceContainer.GameService.Connect();
        }

        private void EntityPositionChanged(EntityPositionChangedEventParameters parameters)
        {
            var entityId = parameters.EntityId;
            var entity = entityContainer.GetRemoteEntity(entityId);

            entity?.GameObject.GetComponent<IPositionSetter>().Move(new Vector2(parameters.X, parameters.Y));
        }
    }
}