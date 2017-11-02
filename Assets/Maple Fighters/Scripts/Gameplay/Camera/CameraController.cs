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

        [Header("Only For Minimap Camera")]
        [SerializeField] private float playerX;
        [SerializeField] private float playerY;

        private void LateUpdate()
        {
            if (Target == null)
            {
                return;
            }

            Move();
        }

        private void Move()
        {
            var x = Mathf.Clamp(Target.position.x, minX, maxX);
            var y = Mathf.Clamp(Target.position.y, minY, maxY);

            var newPosition = new Vector3(playerX + x, playerY + y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }
}