using CommonTools.Log;
using Scripts.Containers;
using Game.Common;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSetter : MonoBehaviour
    {
        private const float SPEED = 15;
        private Vector3 position = Vector3.zero;

        private Transform Character
        {
            get
            {
                if (character == null)
                {
                    var characterBase = GetComponent<CharacterCreatorBase>();
                    character = characterBase?.Character.transform;
                }
                return character;
            }
        }
        private Transform character;
        private ISceneObject sceneObject;

        private void Awake()
        {
            sceneObject = GetComponent<ISceneObject>();
        }

        private void Start()
        {
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.PositionChanged.AddListener(OnPositionChanged);
        }

        private void OnDestroy()
        {
            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.PositionChanged.RemoveListener(OnPositionChanged);
        }

        private void OnPositionChanged(SceneObjectPositionChangedEventParameters parameters)
        {
            var id = parameters.SceneObjectId;
            if (sceneObject.Id != id)
            {
                return;
            }

            var position = new Vector2(parameters.X, parameters.Y);
            var direction = parameters.Direction;
            SetPosition(position, direction);
        }

        private void Update()
        {
            if (position != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, position, SPEED * Time.deltaTime);
            }
        }

        private void SetPosition(Vector2 newPosition, Directions direction)
        {
            position = newPosition;

            FlipByDirection(direction);
        }

        private void FlipByDirection(Directions direction)
        {
            const float SCALE = 1;

            var character = Character;
            if (character == null)
            {
                character = transform;
            }

            switch (direction)
            {
                case Directions.Left:
                {
                    character.localScale = new Vector3(SCALE, character.localScale.y, character.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    character.localScale = new Vector3(-SCALE, character.localScale.y, character.localScale.z);
                    break;
                }
            }
        }
    }
}