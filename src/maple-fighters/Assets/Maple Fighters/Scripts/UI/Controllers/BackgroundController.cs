using System.Linq;
using Scripts.Utils;
using UI.Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI
{
    public class BackgroundController : MonoSingleton<BackgroundController>
    {
        [SerializeField]
        private int[] skipBuildIndexes;

        private BackgroundImage backgroundImage;
        private BackgroundCharacters backgroundCharacters;

        protected override void OnAwake()
        {
            base.OnAwake();

            backgroundImage = UIElementsCreator.GetInstance()
                .Create<BackgroundImage>(UILayer.Background);
            backgroundCharacters = UIElementsCreator.GetInstance()
                .Create<BackgroundCharacters>(UILayer.Background, UIIndex.End);
        }

        private void Start()
        {
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
            if (IsSkippableScene(scene.buildIndex))
            {
                if (backgroundImage != null)
                {
                    Destroy(backgroundImage.gameObject);
                }

                if (backgroundCharacters != null)
                {
                    Destroy(backgroundCharacters.gameObject);
                }

                Destroy(gameObject);
            }
        }

        private bool IsSkippableScene(int index)
        {
            return skipBuildIndexes.Any(buildIndex => buildIndex == index);
        }
    }
}