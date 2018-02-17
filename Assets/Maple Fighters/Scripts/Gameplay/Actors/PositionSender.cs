using CommonCommunicationInterfaces;
using Scripts.Containers;
using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSender : MonoBehaviour
    {
        public Transform Character { get; set; }

        private Vector2 lastPosition;
        private UpdatePositionRequestParameters parameters;

        private void Awake()
        {
            lastPosition = transform.position;

            Directions direction;
            GetDirection(out direction);

            parameters = new UpdatePositionRequestParameters(transform.position.x, transform.position.y, direction);
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, lastPosition) < 0.1f)
            {
                return;
            }

            ChangeParameters();
            SendPositionChangedOperation();

            lastPosition = transform.position;
        }

        private void ChangeParameters()
        {
            Directions direction;
            GetDirection(out direction);

            parameters.X = transform.position.x;
            parameters.Y = transform.position.y;
            parameters.Direction = direction;
        }

        private void SendPositionChangedOperation()
        {
            var messageSendOptions = MessageSendOptions.DefaultUnreliable((byte) GameDataChannels.Position);
            ServiceContainer.GameService.SendOperation((byte)GameOperations.PositionChanged, parameters, messageSendOptions);
        }

        private void GetDirection(out Directions direction)
        {
            if (Character?.localScale.x > 0)
            {
                direction = Directions.Left;
                return;
            }

            if (Character?.localScale.x < 0)
            {
                direction = Directions.Right;
                return;
            }

            direction = Directions.None;
        }
    }
}