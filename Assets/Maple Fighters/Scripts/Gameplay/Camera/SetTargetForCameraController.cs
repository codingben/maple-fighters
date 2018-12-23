using Cinemachine;
using CommonTools.Log;
using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    using Camera = UnityEngine.Camera;

    public class SetTargetForCameraController : MonoBehaviour
    {
        private const string MinimapCameraTag = "Minimap Camera";

        private void Awake()
        {
            SetTargetToCinemachineBrain();
            SetTargetToCameraController();

            Destroy(this);
        }

        private void SetTargetToCinemachineBrain()
        {
            var cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>()
                .AssertNotNull();
            cinemachineBrain.ActiveVirtualCamera.Follow = transform;
        }

        private void SetTargetToCameraController()
        {
            var minimapCamera =
                GameObject.FindGameObjectWithTag(MinimapCameraTag)
                    ?.GetComponent<CameraController>();
            if (minimapCamera != null)
            {
                minimapCamera.SetTarget(transform);
            }
        }
    }
}