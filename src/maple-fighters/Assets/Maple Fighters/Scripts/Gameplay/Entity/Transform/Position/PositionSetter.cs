using System;
using Game.Messages;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    [RequireComponent(typeof(EntityIdentifier))]
    public class PositionSetter : MonoBehaviour
    {
        public event Action<Vector3> PositionChanged;

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

        private IGameApi gameApi;

        private void Awake()
        {
            var entity = GetComponent<IEntity>();
            entityId = entity.Id;
        }

        private void Start()
        {
            gameApi = FindObjectOfType<GameApi>();
            gameApi.PositionChanged += OnPositionChanged;
        }

        private void OnDisable()
        {
            gameApi.PositionChanged -= OnPositionChanged;
        }

        private void OnPositionChanged(PositionChangedMessage message)
        {
            var id = message.GameObjectId;
            var x = message.X;
            var y = message.Y;

            if (entityId == id)
            {
                newPosition = new Vector2(x, y);

                PositionChanged?.Invoke(newPosition);
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