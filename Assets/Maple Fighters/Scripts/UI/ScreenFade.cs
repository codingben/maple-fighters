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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (IsShowed)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}