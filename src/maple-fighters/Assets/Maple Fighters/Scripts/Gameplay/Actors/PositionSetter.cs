using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class PositionSetter : MonoBehaviour
    {
        [Header("Synchronization")]
        [SerializeField]
        private InterpolateOption interpolateOption;

        [SerializeField]
        private float speed;

        [Header("Teleportation")]
        [SerializeField]
        private bool canTeleport;

        [SerializeField]
        private float greaterDistance;

        private Transform character;
        private ISceneObject sceneObject;

        private Vector3 newPosition;

        private void Awake()
        {
            sceneObject = GetComponent<ISceneObject>();

            var spawnedCharacter = GetComponent<ISpawnedCharacter>();
            if (spawnedCharacter != null)
            {
                character = 
                    spawnedCharacter.GetCharacterGameObject().transform;
            }
            else
            {
                character = transform;
            }
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

        private void OnPositionChanged(SceneObjectPositionChangedEventParameters parameters)
        {
            if (sceneObject.Id == parameters.SceneObjectId)
            {
                newPosition = new Vector2(parameters.X, parameters.Y);

                var x = Mathf.Abs(character.localScale.x);

                switch (parameters.Direction)
                {
                    case Directions.Left:
                    {
                        character.localScale = new Vector3(
                            x,
                            character.localScale.y,
                            character.localScale.z);
                        break;
                    }

                    case Directions.Right:
                    {
                        character.localScale = new Vector3(
                            -x,
                            character.localScale.y,
                            character.localScale.z);
                        break;
                    }
                }
            }
        }

        private void Update()
        {
            if (newPosition == Vector3.zero)
            {
                return;
            }

            switch (interpolateOption)
            {
                case InterpolateOption.Disabled:
                {
                    transform.position = newPosition;
                    break;
                }

                case InterpolateOption.Lerp:
                {
                    var distance = Vector2.Distance(transform.position, newPosition);
                    if (distance > greaterDistance)
                    {
                        if (canTeleport)
                        {
                            transform.position = newPosition;
                        }
                    }
                    else
                    {
                        transform.position = 
                            Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
                    }

                    break;
                }
            }
        }
    }
}