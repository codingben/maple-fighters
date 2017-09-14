using Scripts.Containers;
using Scripts.Containers.Service;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class EntityPositionController : MonoBehaviour
    {
        private void Start()
        {
            ServiceContainer.GameService.EntityPositionChanged.AddListener(OnEntityPositionChanged);
        }

        private void OnEntityPositionChanged(EntityPositionChangedEventParameters parameters)
        {
            var entityId = parameters.EntityId;
            var entity = GameContainers.EntityContainer.GetRemoteEntity(entityId);

            entity?.GameObject.GetComponent<IPositionSetter>().SetPosition(new Vector2(parameters.X, parameters.Y), parameters.Direction);
        }
    }
}