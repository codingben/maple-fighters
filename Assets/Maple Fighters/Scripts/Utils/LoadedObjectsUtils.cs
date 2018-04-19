using UnityEngine.SceneManagement;

namespace Scripts.Utils
{
    public static class LoadedObjectsUtils
    {
        public static void GoBackToLogin()
        {
            LoadedObjects.DestroyAll();

            const int LEVEL_INDEX = 0;
            SceneManager.LoadScene(LEVEL_INDEX, LoadSceneMode.Single);
        }
    }
}