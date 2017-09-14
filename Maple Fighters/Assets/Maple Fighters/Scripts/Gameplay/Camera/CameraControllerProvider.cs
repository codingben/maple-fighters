using CommonTools.Log;
using UnityEngine;

namespace Scripts.Gameplay.Camera
{
    public class CameraControllerProvider : MonoBehaviour
    {
        private void Start()
        {
            var cameraController = UnityEngine.Camera.main.GetComponent<CameraController>().AssertNotNull();
            cameraController.Target = transform;
        }
    }
}