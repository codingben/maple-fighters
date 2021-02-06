using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    public class MinimapCameraController : MonoBehaviour
    {
        [Header("Boundaries")]
        [SerializeField]
        private Vector2 minimum;

        [SerializeField]
        private Vector2 maximum;

        [Header("Player Center")]
        [SerializeField]
        private Vector2 center;

        private Transform target;
        private Vector2 oldPosition;

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void LateUpdate()
        {
            MoveCamera();
        }

        private void MoveCamera()
        {
            var x = 0f;
            var y = 0f;

            if (target != null)
            {
                x = Mathf.Clamp(target.position.x, minimum.x, maximum.x);
                y = Mathf.Clamp(target.position.y, minimum.y, maximum.y);

                oldPosition = target.position;
            }

            transform.position = new Vector3(
                center.x + x,
                center.y + y,
                transform.position.z);
        }
    }
}