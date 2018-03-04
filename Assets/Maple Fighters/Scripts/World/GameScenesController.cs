using CommonCommunicationInterfaces;
using Scripts.Containers;
using Scripts.Utils;
using Game.Common;
using UnityEngine.SceneManagement;

namespace Scripts.World
{
    public class GameScenesController : DontDestroyOnLoad<GameScenesController>
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            SubscribeToSceneLoaded();
        }

        private void OnDestroy()
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
            ServiceContainer.GameService.ServiceConnectionHandler.SetNetworkTrafficState(NetworkTrafficState.Flowing);
        }

        public void LoadScene(Maps map)
        {
            ServiceContainer.GameService.ServiceConnectionHandler.SetNetworkTrafficState(NetworkTrafficState.Paused);
            SceneManager.LoadScene((int)map, LoadSceneMode.Single);
        }
    }
}