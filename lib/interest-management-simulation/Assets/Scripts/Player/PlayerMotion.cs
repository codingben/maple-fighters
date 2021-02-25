using UnityEngine;

namespace Game.InterestManagement.Simulation.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotion : MonoBehaviour
    {
        [SerializeField]
        private float speed = 10;
        private new Rigidbody2D rigidbody2D;

        private void Awake()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var direction = new Vector2(horizontal * speed, vertical * speed);
            var newPosition = rigidbody2D.position + direction * Time.fixedDeltaTime;

            rigidbody2D.MovePosition(newPosition);
        }
    }
}