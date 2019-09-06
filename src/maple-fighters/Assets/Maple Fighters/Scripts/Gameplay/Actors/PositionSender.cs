using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class PositionSender : MonoBehaviour
    {
        private Transform character;

        private Vector2 position;
        private Directions direction = Directions.None;

        private void Awake()
        {
            var spawnedCharacter = GetComponent<ISpawnedCharacter>();
            character =
                spawnedCharacter.GetCharacterGameObject().transform;

            position = transform.position;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, position) > 0.1f)
            {
                PositionChanged();
            }
            else
            {
                if (GetDirection() != direction)
                {
                    PositionChanged();
                }
            }
        }

        private void PositionChanged()
        {
            position = transform.position;
            direction = GetDirection();

            UpdatePositionOperation();
        }

        private void UpdatePositionOperation()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.UpdatePosition(
                    new UpdatePositionRequestParameters(
                        transform.position.x,
                        transform.position.y,
                        GetDirection()));
            }
        }

        private Directions GetDirection()
        {
            var direction = Directions.None;

            if (character != null)
            {
                if (character.localScale.x > 0)
                {
                    direction = Directions.Left;
                }

                if (character.localScale.x < 0)
                {
                    direction = Directions.Right;
                }
            }

            return direction;
        }
    }
}