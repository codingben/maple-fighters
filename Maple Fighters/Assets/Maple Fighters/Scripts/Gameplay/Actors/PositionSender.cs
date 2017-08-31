using Scripts.Containers.Service;
using Scripts.Services;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        private IGameService gameService;
        private Vector2 lastPosition;

        private void Awake()
        {
            gameService = ServiceContainer.GameService;
            lastPosition = transform.position;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, lastPosition) < 1)
            {
                return;
            }

            var parameters = new UpdateEntityPositionRequestParameters(transform.position.x, transform.position.y);
            gameService.UpdateEntityPosition(parameters);

            lastPosition = transform.position;
        }
    }
}