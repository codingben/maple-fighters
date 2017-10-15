using System.Threading.Tasks;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Shared.Game.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Services
{
    public class GameConnector : MonoSingleton<GameConnector>
    {
        [SerializeField] private int loadSceneIndex;
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            ServiceContainer.GameService.Connected += OnConnected;
        }

        public void Connect()
        {
            coroutinesExecutor.StartTask(ConnectTask);
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private async Task ConnectTask(IYield yield)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            var connectionStatus = await ServiceContainer.GameService.Connect(yield);
            if (connectionStatus == ConnectionStatus.Failed)
            {
                noticeWindow.Message.text = "Could not connect to a game server.";
                noticeWindow.OkButton.interactable = true;
            }
        }

        private void OnConnected()
        {
            coroutinesExecutor.StartTask(Authenticate);
        }

        private async Task Authenticate(IYield yield)
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();

            var authenticationStatus = await ServiceContainer.GameService.Authenticate(yield);
            if (authenticationStatus == AuthenticationStatus.Failed)
            {
                noticeWindow.Message.text = "Authentication with game server failed.";
                noticeWindow.OkButton.interactable = true;
                return;
            }

            noticeWindow.Hide();

            SceneManager.LoadScene(loadSceneIndex, LoadSceneMode.Single);
        }

        private void OnApplicationQuit()
        {
            ServiceContainer.GameService.Disconnect();
        }
    }
}