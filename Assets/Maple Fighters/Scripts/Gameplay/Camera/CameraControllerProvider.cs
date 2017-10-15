using CommonTools.Log;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Camera
{
    public class CameraControllerProvider : MonoBehaviour
    {
        public void SetCameraTarget()
        {
            var cameraController = UnityEngine.Camera.main.GetComponent<CameraController>().AssertNotNull();
            cameraController.Target = transform;
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
            SetCameraTarget();
        }
    }
}