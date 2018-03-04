using Scripts.UI.Core;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class ScreenFade : UserInterfaceBaseFadeEffect
    {
        private void Start()
        {
            Hide();

            UserInterfaceContainer.Instance.AddOnly(this);
        }

        private void OnEnable()
        {
            SubscribeToSceneLoaded();
        }

        private void OnDisable()
        {
            UnsubscribeFromSceneLoaded();
        }

        private void SubscribeToSceneLoaded()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void UnsubscribeFromSceneLoaded()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (IsShowed)
            {
                Hide();
            }
        }
    }
}