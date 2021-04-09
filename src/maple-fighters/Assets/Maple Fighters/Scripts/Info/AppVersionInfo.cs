using UnityEngine;

namespace Scripts.Info
{
    public class AppVersionInfo : MonoBehaviour
    {
        private void Awake()
        {
            print($"Application version: {Application.version}");
        }
    }
}