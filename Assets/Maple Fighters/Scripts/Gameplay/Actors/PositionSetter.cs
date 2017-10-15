using System;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSetter : MonoBehaviour, IPositionSetter
    {
        public event Action<Directions> DirectionChanged; 

        private const float SPEED = 10;
        private Vector3 position = Vector3.zero;

        private void Update()
        {
            if (position != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, position, SPEED * Time.deltaTime);
            }
        }

        public void SetPosition(Vector2 newPosition, Directions direction)
        {
            position = newPosition;

            FlipByDirection(direction);
        }

        private void FlipByDirection(Directions direction)
        {
            switch (direction)
            {
                case Directions.Left:
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                    break;
                }
                case Directions.None:
                {
                    break;
                }
            }

            DirectionChanged?.Invoke(direction);
        }
    }
}