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

            SceneManager.sceneLoaded += OnSceneLoaded;
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