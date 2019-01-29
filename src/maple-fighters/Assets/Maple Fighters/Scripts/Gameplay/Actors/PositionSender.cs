using Game.Common;
using Scripts.Containers;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        public void SetCharacter(Transform character)
        {
            this.character = character;
        }

        private Transform character;

        private Vector2 position;
        private Directions direction = Directions.None;

        private void Awake()
        {
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
            var gameScenePeerLogic = ServiceContainer.GameService
                .GetPeerLogic<IGameScenePeerLogicAPI>();
            gameScenePeerLogic.UpdatePosition(
                new UpdatePositionRequestParameters(
                    transform.position.x,
                    transform.position.y,
                    GetDirection()));
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