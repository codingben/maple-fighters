using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    [ExecuteInEditMode]
    public class CameraScreenResolution : MonoBehaviour
    {
        [SerializeField] private int targetWidth;
        [SerializeField] private float pixelsToUnits;

        private new UnityEngine.Camera camera;

        private void Awake()
        {
            camera = GetComponent<UnityEngine.Camera>();
        }

        private void Update()
        {
            var height = Mathf.RoundToInt(targetWidth / (float) Screen.width * Screen.height);
            camera.orthographicSize = height / pixelsToUnits / 2;
        }
    }
}