using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class PositionSender : MonoBehaviour
    {
        private Vector2 position;

        private void Awake()
        {
            position = transform.position;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, position) > 0.1f)
            {
                position = transform.position;

                var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
                if (gameSceneApi != null)
                {
                    gameSceneApi.UpdatePosition(new UpdatePositionRequestParameters(transform.position.x, transform.position.y, GetDirection()));
                }
            }
        }
    }
}