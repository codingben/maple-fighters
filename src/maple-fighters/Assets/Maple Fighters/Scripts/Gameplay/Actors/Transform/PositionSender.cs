using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        private const float GreaterDistance = 0.1f;
        private Vector2 position;

        private void Awake()
        {
            position = transform.position;
        }

        private void Update()
        {
            var distance = Vector2.Distance(transform.position, position);
            if (distance > GreaterDistance)
            {
                position = transform.position;

                var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
                if (gameSceneApi != null)
                {
                    // TODO: A way to change the direction
                    var direction = Directions.None;

                    gameSceneApi.UpdatePosition(
                        new UpdatePositionRequestParameters(
                            transform.position.x,
                            transform.position.y,
                            direction));
                }
            }
        }
    }
}