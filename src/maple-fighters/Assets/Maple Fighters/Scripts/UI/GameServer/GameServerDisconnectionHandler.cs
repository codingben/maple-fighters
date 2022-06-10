using NativeWebSocket;
using Scripts.Constants;
using Scripts.Services;
using Scripts.Services.GameApi;
using Scripts.UI.Focus;
using Scripts.UI.Notice;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.UI.GameServer
{
    public class GameServerDisconnectionHandler : MonoBehaviour
    {
        private IGameApi gameApi;

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.Disconnected += OnDisconnected;
        }

        private void OnDestroy()
        {
            gameApi.Disconnected -= OnDisconnected;
        }

        private void OnDisconnected(WebSocketCloseCode code)
        {
            Debug.Log($"Game server disconnection reason: {code}");

            var focusStateController =
                FindObjectOfType<FocusStateController>();
            if (focusStateController != null)
            {
                focusStateController.ChangeFocusState(UIFocusState.UI);
            }

            NoticeUtils.ShowNotice(NoticeMessages.GameServer.ConnectionClosed, OnClicked);
        }

        private void OnClicked()
        {
            SceneManager.LoadScene(sceneName: SceneNames.Main);
        }
    }
}