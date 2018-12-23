using UnityEngine.SceneManagement;

namespace Scripts.Utils
{
    public class SavedGameObjectsUtils : Singleton<SavedGameObjectsUtils>
    {
        private const int DefaultLevelIndex = 0;

        public void GoBackToLogin()
        {
            SavedGameObjects.GetInstance().DestroyAll();
            SceneManager.LoadScene(DefaultLevelIndex, LoadSceneMode.Single);
        }
    }
}