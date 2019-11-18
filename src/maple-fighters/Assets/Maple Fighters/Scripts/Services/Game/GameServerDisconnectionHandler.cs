using CommonCommunicationInterfaces;
using UnityEngine;

namespace Scripts.Services.Game
{
    public class GameServerDisconnectionHandler : MonoBehaviour
    {
        private GameService gameService;

        private void Awake()
        {
            gameService = FindObjectOfType<GameService>();
        }

        private void Start()
        {
            SubscribeToDisconnectionNotifier();
        }

        private void OnDestroy()
        {
            UnsubscribeFromDisconnectionNotifier();
        }

        private void SubscribeToDisconnectionNotifier()
        {
            if (gameService != null)
            {
                gameService.DisconnectionNotifier.Disconnected +=
                    OnDisconnected;
            }
        }

        private void UnsubscribeFromDisconnectionNotifier()
        {
            if (gameService != null)
            {
                gameService.DisconnectionNotifier.Disconnected -=
                    OnDisconnected;
            }
        }

        private void OnDisconnected(DisconnectReason reason, string details)
        {
            // TODO: Implement
        }
    }
}