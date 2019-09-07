using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        private const float GreaterDistance = 0.1f;
        private Vector2 position;

        private ICharacterOrientation characterOrientation;

        private void Awake()
        {
            position = transform.position;
            characterOrientation = GetComponent<ICharacterOrientation>();
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
                    var direction = characterOrientation?.GetDirection();

                    gameSceneApi.UpdatePosition(
                        new UpdatePositionRequestParameters(
                            transform.position.x,
                            transform.position.y,
                            direction ?? Directions.None));
                }
            }
        }
    }
}