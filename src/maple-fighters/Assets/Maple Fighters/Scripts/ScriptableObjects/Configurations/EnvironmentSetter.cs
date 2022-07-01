using System.Runtime.InteropServices;
using Scripts.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ScriptableObjects.Configurations
{
    /// <summary>
    /// This invokes and then invoked by App.jsx (Frontend) to set
    /// one of the hosting environments:
    /// 
    /// 1. Editor
    /// 2. Development
    /// 3. Production
    /// 
    /// Note: If you change invoke method name, make sure to update
    /// the main.jslib file.
    /// </summary>
    public class EnvironmentSetter : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void SetEnv();

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetEnv();
#elif !UNITY_WEBGL || UNITY_EDITOR
            SceneManager.LoadScene(sceneName: SceneNames.Main);
#endif
        }

        public void SetEnvCallback(string environment)
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                networkConfiguration.Environment = ParseEnv(environment);
            }

            Debug.Log($"Set environment: {environment}");

            SceneManager.LoadScene(sceneName: SceneNames.Main);
        }

        private HostingEnvironment ParseEnv(string environment)
        {
            if (environment == "Editor")
            {
                return HostingEnvironment.Editor;
            }
            else if (environment == "Production")
            {
                return HostingEnvironment.Production;
            }

            return HostingEnvironment.Development;
        }
    }
}