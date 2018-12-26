using UnityEngine;

namespace Scripts.World
{
    public class ParallaxScrolling : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private float moveSpeed;

        [Header("Boundaries")]
        [SerializeField]
        private float minX;

        [SerializeField]
        private float maxX;

        [SerializeField]
        private float minY;

        [SerializeField]
        private float maxY;

        private Transform target;

        private void Awake()
        {
            target = Camera.main.transform;
        }

        private void Update()
        {
            if (target != null)
            {
                var newPosition = new Vector3(
                    Mathf.Clamp(target.position.x, minX, maxX),
                    Mathf.Clamp(target.position.x, minY, maxY),
                    transform.position.z);

                transform.position = Vector3.LerpUnclamped(
                    transform.position,
                    newPosition,
                    moveSpeed * Time.deltaTime);
            }
        }
    }
}