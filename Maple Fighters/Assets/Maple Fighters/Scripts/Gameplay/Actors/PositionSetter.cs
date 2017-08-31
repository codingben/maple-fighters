using CommonTools.Log;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PositionSetter : MonoBehaviour, IPositionSetter
    {
        private const float SPEED = 5;

        private new Rigidbody2D rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>().AssertNotNull();
        }

        public void Move(Vector2 position)
        {
            var transformPosition = new Vector2(transform.position.x, transform.position.y);
            Vector3 direction = (position - transformPosition).normalized;
            rigidbody.MovePosition(transform.position + direction * SPEED * Time.deltaTime);
        }
    }
}