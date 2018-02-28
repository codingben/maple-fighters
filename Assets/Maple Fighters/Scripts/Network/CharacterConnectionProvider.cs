using System;
using Character.Client.Common;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.ScriptableObjects;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.Services
{
    public class CharacterConnectionProvider : ServiceConnectionProvider<CharacterConnectionProvider>
    {
        private Action onAuthorized;

        public void Connect(Action onAuthorized)
        {
            this.onAuthorized = onAuthorized;

            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Character);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, ServiceContainer.CharacterService, connectionInformation));
        }

        protected override void OnPreConnection()
        {
            // Left blank intentionally
        }

        protected override void OnConnectionFailed()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Could not connect to a character service.";
            noticeWindow.OkButton.interactable = true;
        }

        protected override void OnConnectionEstablished()
        {
            CoroutinesExecutor.StartTask((yield) => Authorize(yield, (byte)CharacterOperations.Authorize));
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Authentication with character service failed.";
            noticeWindow.OkButton.interactable = true;

            ServiceContainer.GameService.Dispose();
        }

        protected override void OnAuthorized()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Hide();

            onAuthorized?.Invoke();
        }
    }
}