using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        private const float GreaterDistance = 0.1f;
        private Vector2 lastPosition;

        private void Awake()
        {
            lastPosition = transform.position;
        }

        private void Update()
        {
            var distance = Vector2.Distance(transform.position, lastPosition);
            if (distance > GreaterDistance)
            {
                lastPosition = transform.position;

                var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
                if (gameSceneApi != null)
                {
                    var z = GetDirection();

                    gameSceneApi.UpdatePosition(
                        new UpdatePositionRequestParameters(
                            transform.position.x, transform.position.y, z));
                }
            }
        }

        private Directions GetDirection()
        {
            var position = new Vector3(lastPosition.x, lastPosition.y);
            var orientation = (transform.position - position).normalized;
            var direction = orientation.x < 0 ? Directions.Left : Directions.Right;

            return direction;
        }
    }
}