using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class PositionSetter : MonoBehaviour
    {
        private const float Speed = 15;
        
        private Transform character;
        private ISceneObject sceneObject;

        private Vector3 position;

        private void Awake()
        {
            sceneObject = GetComponent<ISceneObject>();

            var spawnedCharacter = GetComponent<ISpawnedCharacter>();
            character =
                spawnedCharacter.GetCharacterGameObject().transform;
        }

        private void Start()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.PositionChanged.AddListener(OnPositionChanged);
            }
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.PositionChanged.RemoveListener(OnPositionChanged);
            }
        }

        private void OnPositionChanged(
            SceneObjectPositionChangedEventParameters parameters)
        {
            if (sceneObject.Id == parameters.SceneObjectId)
            {
                UpdatePosition(
                    new Vector2(parameters.X, parameters.Y),
                    parameters.Direction);
            }
        }

        private void Update()
        {
            if (position != Vector3.zero)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    position,
                    Speed * Time.deltaTime);
            }
        }

        public void UpdatePosition(Vector2 newPosition, Directions direction)
        {
            position = newPosition;

            FlipByDirection(direction);
        }

        private void FlipByDirection(Directions direction)
        {
            const float Scale = 1;

            switch (direction)
            {
                case Directions.Left:
                {
                    character.localScale = new Vector3(
                        Scale,
                        character.localScale.y,
                        character.localScale.z);
                    break;
                }

                case Directions.Right:
                {
                    character.localScale = new Vector3(
                        -Scale,
                        character.localScale.y,
                        character.localScale.z);
                    break;
                }
            }
        }
    }
}