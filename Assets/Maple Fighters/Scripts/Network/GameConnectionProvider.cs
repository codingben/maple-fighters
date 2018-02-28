using System;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.ScriptableObjects;
using Scripts.UI.Core;
using Scripts.UI.Windows;
using Shared.Game.Common;

namespace Scripts.Services
{
    public class GameConnectionProvider : ServiceConnectionProvider<GameConnectionProvider>
    {
        private Action onAuthorized;

        public void Connect(Action onAuthorized)
        {
            this.onAuthorized = onAuthorized;

            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Game);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, ServiceContainer.GameService, connectionInformation));
        }

        protected override void OnPreConnection()
        {
            // Left blank intentionally
        }

        protected override void OnConnectionFailed()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Could not connect to a game server.";
            noticeWindow.OkButton.interactable = true;
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask((yield) => Authorize(yield, (byte)GameOperations.Authorize));
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Authentication with game server failed.";
            noticeWindow.OkButton.interactable = true;

            ServiceContainer.GameService.Dispose();
        }

        protected override void OnAuthorized()
        {
            onAuthorized?.Invoke();
        }
    }
}