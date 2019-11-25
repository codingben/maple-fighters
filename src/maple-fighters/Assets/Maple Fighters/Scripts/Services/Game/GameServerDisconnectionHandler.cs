using CommonCommunicationInterfaces;
using Scripts.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Services.Game
{
    [RequireComponent(typeof(GameService))]
    public class GameServerDisconnectionHandler : MonoBehaviour
    {
        private GameService gameService;

        private void Awake()
        {
            gameService = GetComponent<GameService>();
        }

        private void Start()
        {
            SubscribeToGameServiceEvents();
        }

        private void OnDestroy()
        {
            UnubscribeFromGameServiceEvents();
            UnsubscribeFromDisconnectionNotifier();
        }

        private void SubscribeToGameServiceEvents()
        {
            gameService.Connected += OnConnected;
        }

        private void UnubscribeFromGameServiceEvents()
        {
            gameService.Connected -= OnConnected;
        }

        private void OnConnected()
        {
            SubscribeToDisconnectionNotifier();
        }

        private void SubscribeToDisconnectionNotifier()
        {
            if (gameService.DisconnectionNotifier != null)
            {
                gameService.DisconnectionNotifier.Disconnected +=
                    OnDisconnected;
            }
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            if (gameService.DisconnectionNotifier != null)
            {
                gameService.DisconnectionNotifier.Disconnected -=
                    OnDisconnected;
            }
        }

        private void OnDisconnected(DisconnectReason reason, string details)
        {
            UnsubscribeFromDisconnectionNotifier();

            SceneManager.LoadScene(SceneNames.Main);
        }
    }
}