using System;
using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Containers;
using Scripts.ScriptableObjects;
using Scripts.Services;
using Scripts.UI.Core;
using Scripts.UI.Windows;

namespace Scripts.UI.Controllers
{
    public class LoginConnectionProvider : ServiceConnectionProvider<LoginConnectionProvider>
    {
        private Action onConnected;

        public void Connect(Action onConnected)
        {
            this.onConnected = onConnected;

            var connectionInformation = ServicesConfiguration.GetInstance().GetConnectionInformation(ServersType.Login);
            CoroutinesExecutor.StartTask((yield) => Connect(yield, ServiceContainer.LoginService, connectionInformation));
        }

        protected override void OnPreConnection()
        {
            // Left blank intentionally
        }

        protected override void OnConnectionFailed()
        {
            var noticeWindow = UserInterfaceContainer.Instance.Get<NoticeWindow>().AssertNotNull();
            noticeWindow.Message.text = "Could not connect to a login server.";
            noticeWindow.OkButton.interactable = true;
        }

        protected override void OnConnectionEstablished()
        {
            onConnected?.Invoke();
        }

        protected override void OnPreAuthorization()
        {
            // Left blank intentionally
        }

        protected override void OnNonAuthorized()
        {
            // Left blank intentionally
        }

        protected override void OnAuthorized()
        {
            // Left blank intentionally
        }
    }
}