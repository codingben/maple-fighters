using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.ScriptableObjects;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Shared.Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Services
{
    public class GameConnector : ServiceConnector<GameConnector>
    {
        [SerializeField] private int loadSceneIndex;

        protected override void OnAwake()
        {
            base.OnAwake();

            DontDestroyOnLoad();
        }

        public void Connect()
        {
            CoroutinesExecutor.StartTask(Connect);
        }

        private async Task Connect(IYield yield)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();

            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Game);
            var connectionStatus = await Connect(yield, ServiceContainer.GameService, connectionInformation);
            if (connectionStatus == ConnectionStatus.Failed)
            {
                noticeWindow.Message.text = "Could not connect to a game server.";
                noticeWindow.OkButton.interactable = true;
                return;
            }

            CoroutinesExecutor.StartTask(Authenticate);
        }

        private async Task Authenticate(IYield yield)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();

            var authenticationStatus = await ServiceContainer.GameService.Authenticate(yield);
            if (authenticationStatus == AuthenticationStatus.Failed)
            {
                ServiceContainer.GameService.Dispose();

                noticeWindow.Message.text = "Authentication with game server failed.";
                noticeWindow.OkButton.interactable = true;
                return;
            }

            noticeWindow.Hide();

            SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);
        }
    }
}