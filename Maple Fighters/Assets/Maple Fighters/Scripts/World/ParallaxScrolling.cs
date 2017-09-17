using CommonTools.Log;
using UnityEngine;

namespace Scripts.World
{
    public class ParallaxScrolling : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private float moveSpeed;

        [Header("Boundaries")]
        [SerializeField] private float minX;
        [SerializeField] private float maxX;
        [SerializeField] private float minY;
        [SerializeField] private float maxY;

        private Transform target;

        private void Start()
        {
            target = Camera.main.transform.AssertNotNull();
        }

        private void Update()
        {
            if (target == null)
            {
                return;
            }

            var x = Mathf.Clamp(target.position.x, minX, maxX);
            var y = Mathf.Clamp(target.position.x, minY, maxY);
            var newPosition = new Vector3(x, y, transform.position.z);

            transform.position = Vector3.LerpUnclamped(transform.position, newPosition, moveSpeed * Time.deltaTime);
        }
    }
}