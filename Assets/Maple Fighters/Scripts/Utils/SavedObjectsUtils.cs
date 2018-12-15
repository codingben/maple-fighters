using UnityEngine.SceneManagement;

namespace Scripts.Utils
{
    public static class SavedObjectsUtils
    {
        public static void GoBackToLogin()
        {
            SavedObjects.DestroyAll();

            const int LEVEL_INDEX = 0;
            SceneManager.LoadScene(LEVEL_INDEX, LoadSceneMode.Single);
        }
    }
}