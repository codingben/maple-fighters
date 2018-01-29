using CommonTools.Log;
using UnityEngine;
using Cinemachine;

namespace Scripts.Gameplay.Camera
{
    using Camera = UnityEngine.Camera;

    public class SetTargetForCameraController : MonoBehaviour
    {
        private void Awake()
        {
            var cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>().AssertNotNull();
            cinemachineBrain.ActiveVirtualCamera.Follow = transform;

            const string MINIMAP_CAMERA_TAG = "Minimap Camera";
            var minimapCamera = GameObject.FindGameObjectWithTag(MINIMAP_CAMERA_TAG)?.GetComponent<CameraController>();
            if (minimapCamera != null)
            {
                minimapCamera.Target = transform;
            }

            Destroy(this);
        }
    }
}