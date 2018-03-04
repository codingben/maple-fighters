using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform Target { get; set; }

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

        private void LateUpdate()
        {
            if (Target != null)
            {
                Move();
            }
        }

        private void Move()
        {
            var x = Mathf.Clamp(Target.position.x, minX, maxX);
            var y = Mathf.Clamp(Target.position.y, minY, maxY);

            var newPosition = new Vector3(centerX + x, centerY + y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }
}