using CommonTools.Log;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerController : MonoBehaviour
    {
        private enum PlayerState
        {
            Moving,
            Falling
        }

        [SerializeField] private float speed;
        [SerializeField] private PlayerState playerState = PlayerState.Falling;

        private Vector2 position;
        private new Rigidbody2D rigidbody;

        private LayerMask floorLayerMask;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>().AssertNotNull();
            floorLayerMask = LayerMask.GetMask("Floor");
        }

        private void FixedUpdate()
        {
            switch (playerState)
            {
                case PlayerState.Moving:
                {
                    MovingState();
                    break;
                }
                case PlayerState.Falling:
                {
                    FallingState();
                    break;
                }
            }
        }

        public void Move(Vector2 direction)
        {
            var transformPosition = new Vector2(rigidbody.position.x, rigidbody.position.y);
            position = transformPosition + direction * speed * Time.deltaTime;
        }

        private void MovingState()
        {
            if (!IsOnFloor())
            {
                playerState = PlayerState.Falling;
                return;
            }

            rigidbody.MovePosition(position);
        }

        private void FallingState()
        {
            if (IsOnFloor())
            {
                playerState = PlayerState.Moving;
            }
        }

        private bool IsOnFloor() => Physics2D.Raycast(rigidbody.position, Vector2.down, 0.5f, floorLayerMask);
    }
}