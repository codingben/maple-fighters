using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    using Camera = UnityEngine.Camera;

    #pragma warning disable 0109

    [ExecuteInEditMode]
    public class CameraScreenResolution : MonoBehaviour
    {
        [SerializeField] private int targetWidth;
        [SerializeField] private float pixelsToUnits;

        private new Camera camera;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        private void Update()
        {
            var height = Mathf.RoundToInt(targetWidth / (float) Screen.width * Screen.height);
            camera.orthographicSize = height / pixelsToUnits / 2;
        }
    }
}