using Scripts.Containers;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        public Transform Character { get; set; }
        private Vector2 lastPosition;

        private void Awake()
        {
            lastPosition = transform.position;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, lastPosition) > 0.1f)
            {
                PositionChanged();
            }
        }

        private void PositionChanged()
        {
            lastPosition = transform.position;
            UpdatePositionOperation();
        }

        private void UpdatePositionOperation()
        {
            var parameters = new UpdatePositionRequestParameters(transform.position.x, transform.position.y, GetDirection());
            ServiceContainer.GameService.UpdatePosition(parameters);
        }

        private Directions GetDirection()
        {
            if (Character?.localScale.x > 0)
            {
                return Directions.Left;
            }

            if (Character?.localScale.x < 0)
            {
                return Directions.Right;
            }

            {
                return Directions.None;
            }
        }
    }
}