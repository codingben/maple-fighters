using System.Linq;
using Scripts.UI.Core;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class BackgroundController : DontDestroyOnLoad<BackgroundController>
    {
        [SerializeField] private int[] skipBuildIndexes;

        private BackgroundImage backgroundImage;
        private BackgroundCharactersParent backgroundCharactersParent;

        private void Start()
        {
            backgroundImage = UserInterfaceContainer.Instance.Add<BackgroundImage>(ViewType.Background);
            backgroundCharactersParent = UserInterfaceContainer.Instance.Add<BackgroundCharactersParent>(ViewType.Background, Index.Last);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            var index = scene.buildIndex;
            if (IsSkippableScene(index))
            {
                return;
            }

            UserInterfaceContainer.Instance.Remove(backgroundImage);
            UserInterfaceContainer.Instance.Remove(backgroundCharactersParent);

            Destroy(gameObject);
        }

        private bool IsSkippableScene(int index)
        {
            return skipBuildIndexes.Any(buildIndex => buildIndex == index);
        }
    }
}