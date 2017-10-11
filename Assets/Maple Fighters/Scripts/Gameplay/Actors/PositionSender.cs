using Scripts.Containers.Service;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        private Vector2 lastPosition;

        private void Awake()
        {
            lastPosition = transform.position;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, lastPosition) < 0.1f)
            {
                return;
            }

            Directions direction;
            GetDirection(out direction);

            var parameters = new UpdatePositionRequestParameters(transform.position.x, transform.position.y, direction);
            ServiceContainer.GameService.UpdatePosition(parameters);

            lastPosition = transform.position;
        }

        private void GetDirection(out Directions direction)
        {
            if (transform.localScale.x > 0)
            {
                direction = Directions.Left;
                return;
            }

            if (transform.localScale.x < 0)
            {
                direction = Directions.Right;
                return;
            }

            direction = Directions.None;
        }
    }
}