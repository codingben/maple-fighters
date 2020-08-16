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
                var newPosition = new Vector3(
                    background.position.x + (mainCamera.position.x - cameraPosition.x),
                    background.position.y + (mainCamera.position.y - cameraPosition.y),
                    background.position.z);

                background.position =
                    Vector3.Lerp(background.position, newPosition, speed * Time.deltaTime);
            }

            cameraPosition = mainCamera.position;
        }
    }
}