using CommonTools.Log;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Camera
{
    public class CameraControllerProvider : MonoBehaviour
    {
        private const string MINI_CAMERA_TAG = "Minimap Camera";

        public void SetCamerasTarget()
        {
            var cameraController = UnityEngine.Camera.main.GetComponent<CameraController>().AssertNotNull();
            cameraController.Target = transform;

            var miniCameraController = GameObject.FindGameObjectWithTag(MINI_CAMERA_TAG)?.GetComponent<CameraController>();
            if (miniCameraController != null)
            {
                miniCameraController.Target = transform;
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SetCamerasTarget();
        }
    }
}