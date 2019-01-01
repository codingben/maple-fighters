using System.Linq;
using Scripts.UI.Core;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class BackgroundController : MonoSingleton<BackgroundController>
    {
        [SerializeField] private int[] skipBuildIndexes;

        private BackgroundImage backgroundImage;
        private BackgroundCharacters backgroundCharacters;

        private void Start()
        {
            backgroundImage = UserInterfaceContainer.GetInstance().Add<BackgroundImage>(ViewType.Background);
            backgroundCharacters = UserInterfaceContainer.GetInstance().Add<BackgroundCharacters>(ViewType.Background, Index.Last);

            SubscribeToSceneLoaded();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

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
            var index = scene.buildIndex;
            if (IsSkippableScene(index))
            {
                return;
            }

            UserInterfaceContainer.GetInstance()?.Remove(backgroundImage);
            UserInterfaceContainer.GetInstance()?.Remove(backgroundCharacters);

            Destroy(gameObject);
        }

        private bool IsSkippableScene(int index)
        {
            return skipBuildIndexes.Any(buildIndex => buildIndex == index);
        }
    }
}