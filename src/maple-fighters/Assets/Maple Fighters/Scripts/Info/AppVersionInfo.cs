using UnityEngine;

namespace Scripts.Info
{
    public class AppVersionInfo : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log($"Application version: {Application.version}");
            Debug.Log($"Unity version: {Application.unityVersion}");
        }
    }
}