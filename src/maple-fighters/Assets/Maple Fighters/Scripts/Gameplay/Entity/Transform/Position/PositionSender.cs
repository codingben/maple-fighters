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

                var message = new ChangePositionMessage()
                {
                    X = x,
                    Y = y
                };

                gameApi?.SendMessage(MessageCodes.ChangePosition, message);
            }
        }
    }
}