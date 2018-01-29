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
            StartCoroutine(WaitFrame());
        }

        private IEnumerator WaitFrame()
        {
            yield return null;

            var loginWindow = UserInterfaceContainer.Instance.Get<LoginWindow>().AssertNotNull();
            loginWindow.Email = QuickLoginConfiguration.GetInstance().Email;
            loginWindow.Password = QuickLoginConfiguration.GetInstance().Password;
        }
        #endif
    }
}