using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private float moveSpeed;

        [Header("Boundaries")]
        [SerializeField]
        private Vector2 minimum;

        [SerializeField]
        private Vector2 maximum;

        [Header("Player Center")]
        [SerializeField]
        private Vector2 center;

        private Transform target;

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void LateUpdate()
        {
            if (target != null)
            {
                Move();
            }
        }

        private void Move()
        {
            var x = Mathf.Clamp(target.position.x, minimum.x, maximum.x);
            var y = Mathf.Clamp(target.position.y, minimum.y, maximum.y);

            var newPosition = new Vector3(
                center.x + x,
                center.y + y,
                transform.position.z);

            transform.position = Vector3.Lerp(
                transform.position,
                newPosition,
                moveSpeed * Time.deltaTime);
        }
    }
}