using System.Collections;
using CommonTools.Log;
using Scripts.ScriptableObjects;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using UnityEngine;

namespace Scripts.QuickAccess
{
    public class QuickLogin : MonoBehaviour
    {
        #if UNITY_EDITOR
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
                var loginWindow = 
                    UserInterfaceContainer.GetInstance().Get<LoginWindow>()
                        .AssertNotNull();
                loginWindow.Email = quickLoginConfiguration.Email;
                loginWindow.Password = quickLoginConfiguration.Password;
            }

            Destroy(gameObject);
        }
        #endif
    }
}