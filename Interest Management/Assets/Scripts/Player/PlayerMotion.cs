using UnityEngine;

namespace Game.InterestManagement.Simulation.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotion : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10;

        [SerializeField]
        private float jumpHeight = 7.5f;

        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Move();
            Jump();
        }

        private void Move()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var direction = new Vector2(horizontal * speed, 0);
            rigidbody2D.AddForce(direction, ForceMode2D.Force);
        }

        private void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var direction = new Vector2(0, jumpHeight);
                rigidbody2D.AddForce(direction, ForceMode2D.Impulse);
            }
        }
    }
}