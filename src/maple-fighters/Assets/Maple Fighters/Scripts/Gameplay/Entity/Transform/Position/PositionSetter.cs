using System;
using Game.Common;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    [RequireComponent(typeof(EntityIdentifier))]
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

        private GameService gameService;

        private void Awake()
        {
            var entity = GetComponent<IEntity>();
            entityId = entity.Id;
        }

        private void Start()
        {
            gameService = FindObjectOfType<GameService>();
            gameService?.GameSceneApi.PositionChanged.AddListener(OnPositionChanged);
        }

        private void OnDisable()
        {
            gameService?.GameSceneApi.PositionChanged.RemoveListener(OnPositionChanged);
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
                    var distance = 
                        Vector2.Distance(transform.position, newPosition);
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