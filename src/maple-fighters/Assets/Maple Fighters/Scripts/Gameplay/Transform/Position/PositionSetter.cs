using System;
using Game.Common;
using Scripts.Gameplay.GameEntity;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(Entity))]
    public class PositionSetter : MonoBehaviour
    {
        public event Action<Directions> DirectionChanged;

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

        private int entityId;
        private Vector3 newPosition;

        private void Awake()
        {
            var entity = GetComponent<IEntity>();
            entityId = entity.Id;
        }

        private void Start()
        {
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.PositionChanged.AddListener(OnPositionChanged);
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.PositionChanged.RemoveListener(OnPositionChanged);
        }

        private void OnPositionChanged(SceneObjectPositionChangedEventParameters parameters)
        {
            if (entityId == parameters.SceneObjectId)
            {
                newPosition = new Vector2(parameters.X, parameters.Y);

                DirectionChanged?.Invoke(parameters.Direction);
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