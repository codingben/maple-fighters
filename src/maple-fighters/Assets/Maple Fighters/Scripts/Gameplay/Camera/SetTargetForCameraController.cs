using Cinemachine;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    public class SetTargetForCameraController : MonoBehaviour
    {
        private void Awake()
        {
            SetTargetToCinemachineBrain();
            SetTargetToCameraController();

            Destroy(this);
        }

        private void SetTargetToCinemachineBrain()
        {
            var cinemachineBrain =
                UnityEngine.Camera.main.GetComponent<CinemachineBrain>();
            if (cinemachineBrain != null)
            {
                cinemachineBrain.ActiveVirtualCamera.Follow = transform;
            }
        }

        private void SetTargetToCameraController()
        {
            var minimapCamera =
                GameObject.FindGameObjectWithTag(GameTags.MinimapCameraTag);
            var cameraController =
                minimapCamera?.GetComponent<CameraController>();

            cameraController?.SetTarget(transform);
        }
    }
}