using CommonTools.Log;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSetter : MonoBehaviour, IPositionSetter
    {
        private const float SPEED = 10;

        // private new Rigidbody2D rigidbody;
        private Vector3 position = Vector3.zero;

        private void Awake()
        {
            // rigidbody = GetComponent<Rigidbody2D>().AssertNotNull();
        }

        private void Update()
        {
            if (position != Vector3.zero)
            {
                transform.position = Vector3.Lerp(transform.position, position, SPEED * Time.deltaTime);
            }
        }

        public void Move(Vector2 position)
        {
            /*var transformPosition = new Vector2(transform.position.x, transform.position.y);
            Vector3 direction = (position - transformPosition).normalized;
            rigidbody.MovePosition(transform.position + direction * SPEED * Time.deltaTime);*/
            this.position = position;
        }
    }
}