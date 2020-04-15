using System.Collections;
using ScriptableObjects.Configurations;
using Scripts.UI.Authenticator;
using UnityEngine;

namespace Scripts.QuickAccess
{
    public class QuickLogin : MonoBehaviour
    {
        #if UNITY_EDITOR || UNITY_WEBGL
        private void Start()
        {
            StartCoroutine(WaitFrameBeforeDestroy());
        }

        private IEnumerator WaitFrameBeforeDestroy()
        {
            yield return null;

            var quickLoginConfiguration = QuickLoginConfiguration.GetInstance();
            if (quickLoginConfiguration != null)
            {
                var loginWindow = FindObjectOfType<LoginWindow>();
                if (loginWindow != null)
                {
                    loginWindow.Email = quickLoginConfiguration.Email;
                    loginWindow.Password = quickLoginConfiguration.Password;
                }
            }

            Destroy(gameObject);
        }
        #endif
    }
}