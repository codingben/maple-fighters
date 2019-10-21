using Game.Common;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.EntityTransform
{
    public class PositionSender : MonoBehaviour
    {
        [SerializeField]
        private float greaterDistance = 0.1f;
        private Vector2 lastPosition;

        private IGameService gameService;

        private void Awake()
        {
            lastPosition = transform.position;
        }

        private void Start()
        {
            gameService = GameService.GetInstance();
        }

        private void Update()
        {
            var distance = Vector2.Distance(transform.position, lastPosition);
            if (distance > greaterDistance)
            {
                lastPosition = transform.position;

                if (gameService != null)
                {
                    var x = transform.position.x;
                    var y = transform.position.y;
                    var z = GetDirection();

                    var parameters =
                        new UpdatePositionRequestParameters(x, y, z);

                    gameService.GameSceneApi.UpdatePosition(parameters);
                }
            }
        }

        private Directions GetDirection()
        {
            var position = new Vector3(lastPosition.x, lastPosition.y);
            var orientation = (transform.position - position).normalized;
            var direction = orientation.x < 0 ? Directions.Left : Directions.Right;

            return direction;
        }
    }
}