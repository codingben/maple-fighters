using CommonCommunicationInterfaces;
using Game.Common;
using Scripts.Containers;
using Scripts.Utils;
using UnityEngine.SceneManagement;

namespace Scripts.World
{
    public class GameScenesController : MonoSingleton<GameScenesController>
    {
        protected override void OnAwake()
        {
            base.OnAwake();

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
            ServiceContainer.GameService.ServiceConnectionHandler
                .SetNetworkTrafficState(NetworkTrafficState.Flowing);
        }

        public void LoadScene(Maps map)
        {
            ServiceContainer.GameService.ServiceConnectionHandler
                .SetNetworkTrafficState(NetworkTrafficState.Paused);

            SceneManager.LoadScene(map.ToString());
        }
    }
}