using CommonCommunicationInterfaces;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Services.Utils
{
    public class GameNetworkTrafficStateSetter : MonoBehaviour
    {
        private IGameService gameService;

        private void Awake()
        {
            gameService = GameService.GetInstance();
            gameService?.SetNetworkTrafficState(NetworkTrafficState.Flowing);
        }

        private void OnDestroy()
        {
            gameService?.SetNetworkTrafficState(NetworkTrafficState.Paused);
        }
    }
}