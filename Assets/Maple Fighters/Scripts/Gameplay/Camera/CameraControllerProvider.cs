using CommonTools.Log;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Camera
{
    public class CameraControllerProvider : MonoBehaviour
    {
        private void Start()
        {
            SetCameraTarget();
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

        private void SetCameraTarget()
        {
            var cameraController = UnityEngine.Camera.main.GetComponent<CameraController>().AssertNotNull();
            cameraController.Target = transform;
        }
    }
}