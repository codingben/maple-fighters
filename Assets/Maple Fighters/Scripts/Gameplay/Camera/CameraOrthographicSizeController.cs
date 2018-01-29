using UnityEngine;
using Cinemachine;

namespace Scripts.Gameplay.Camera
{
    #pragma warning disable 0109

    [ExecuteInEditMode]
    public class CameraOrthographicSizeController : MonoBehaviour
    {
        [SerializeField] private int targetWidth;
        [SerializeField] private float pixelsToUnits;

        private new CinemachineVirtualCamera camera;

        private void Awake()
        {
            camera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            if (camera == null)
            {
                camera = GetComponent<CinemachineVirtualCamera>();
            }

            var height = Mathf.RoundToInt(targetWidth / (float) Screen.width * Screen.height);
            camera.m_Lens.OrthographicSize = height / pixelsToUnits / 2;
        }
    }
}