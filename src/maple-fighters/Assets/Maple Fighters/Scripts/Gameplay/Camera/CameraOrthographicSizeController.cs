using Cinemachine;
using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    #pragma warning disable 0109

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraOrthographicSizeController : MonoBehaviour
    {
        [SerializeField]
        private int targetWidth;

        [SerializeField]
        private float pixelsToUnits;

        private new CinemachineVirtualCamera camera;

        private void Awake()
        {
            camera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            var dimension = (targetWidth / (float)Screen.width) * Screen.height;
            var height = Mathf.RoundToInt(dimension);

            camera.m_Lens.OrthographicSize = height / pixelsToUnits / 2;
        }
    }
}