using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class ParallaxScrollingEffect : MonoBehaviour
    {
        [SerializeField]
        private Transform[] backgrounds;

        [SerializeField]
        private float speed;

        private Transform mainCamera;
        private Vector3 cameraPosition;

        private void Awake()
        {
            mainCamera = UnityEngine.Camera.main.transform;
            cameraPosition = mainCamera.position;
        }

        private void Update()
        {
            foreach (var background in backgrounds)
            {
                var x = background.position.x;
                var y = background.position.y;
                var z = background.position.z;
                var a = background.position;
                var b = new Vector3(
                    x + (mainCamera.position.x - cameraPosition.x),
                    y + (mainCamera.position.y - cameraPosition.y),
                    z);
                var t = speed * Time.deltaTime;

                background.position = Vector3.Lerp(a, b, t);
            }

            cameraPosition = mainCamera.position;
        }
    }
}