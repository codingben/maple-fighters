using Game.Common;
using Game.Messages;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Entity
{
    public class PositionSender : MonoBehaviour
    {
        private IGameApi gameApi;

        [SerializeField]
        private float greaterDistance = 0.1f;
        private Vector2 lastPosition;

        private void Awake()
        {
            lastPosition = transform.position;
        }

        private void Start()
        {
            gameApi = FindObjectOfType<GameApi>();
        }

        private void Update()
        {
            var distance = Vector2.Distance(transform.position, lastPosition);
            if (distance > greaterDistance)
            {
                lastPosition = transform.position;

                var x = transform.position.x;
                var y = transform.position.y;
                // var z = GetDirection();

                var parameters =
                    new ChangePositionMessage()
                    {
                        X = x,
                        Y = y
                    };

                gameApi?.ChangePosition(parameters);
            }
        }

        // TODO: Use this method
        private Directions GetDirection()
        {
            var position = new Vector3(lastPosition.x, lastPosition.y);
            var orientation = (transform.position - position).normalized;
            var direction = orientation.x < 0 ? Directions.Left : Directions.Right;

            return direction;
        }
    }
}