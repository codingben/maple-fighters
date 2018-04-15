using UnityEngine;

namespace InterestManagement.Scripts
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;

        private void Awake()
        {
            if (!target)
            {
                Debug.LogWarning("CameraFollow::Awake() -> Target is null.");
            }
        }

        private void LateUpdate()
        {
            if (target)
            {
                Move();
            }
        }

        private void Move()
        {
            var newPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.smoothDeltaTime);
        }
    }
}