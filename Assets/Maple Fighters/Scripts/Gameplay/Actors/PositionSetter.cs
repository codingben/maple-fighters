using System;
using CommonTools.Log;
using Scripts.Containers;
using Game.Common;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSetter : MonoBehaviour
    {
        public event Action<Directions> DirectionChanged; 

        private const float SPEED = 15;
        private Vector3 position = Vector3.zero;

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

            switch (direction)
            {
                case Directions.Left:
                {
                    transform.localScale = new Vector3(SCALE, transform.localScale.y, transform.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    transform.localScale = new Vector3(-SCALE, transform.localScale.y, transform.localScale.z);
                    break;
                }
            }

            DirectionChanged?.Invoke(direction);
        }
    }
}