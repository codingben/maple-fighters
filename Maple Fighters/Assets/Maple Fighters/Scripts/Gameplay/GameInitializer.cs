using Scripts.Containers;
using Scripts.Containers.Service;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            var gameService = ServiceContainer.GameService;
            var entityContainer = GameContainers.EntityContainer;
        }

        private void Start()
        {
            // ServiceContainer.GameService.Connect();
        }
    }
}