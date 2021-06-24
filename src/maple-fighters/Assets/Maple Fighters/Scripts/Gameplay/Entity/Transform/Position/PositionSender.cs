using Game.Messages;
using Scripts.Services;
using Scripts.Services.GameApi;
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
            gameApi = ApiProvider.ProvideGameApi();
        }

        private void Update()
        {
            var distance = Vector2.Distance(transform.position, lastPosition);
            if (distance > greaterDistance)
            {
                lastPosition = transform.position;

                var message = new ChangePositionMessage()
                {
                    X = transform.position.x,
                    Y = transform.position.y
                };

                gameApi?.SendMessage(MessageCodes.ChangePosition, message);
            }
        }
    }
}