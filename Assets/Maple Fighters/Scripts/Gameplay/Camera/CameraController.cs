using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private float moveSpeed;

        [Header("Boundaries")]
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        [Header("Player Center")]
        [SerializeField] private float centerX;
        [SerializeField] private float centerY;

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
            var x = Mathf.Clamp(target.position.x, minX, maxX);
            var y = Mathf.Clamp(target.position.y, minY, maxY);

            var newPosition = new Vector3(
                centerX + x,
                centerY + y,
                transform.position.z);
            transform.position = Vector3.Lerp(
                transform.position,
                newPosition,
                moveSpeed * Time.deltaTime);
        }
    }
}