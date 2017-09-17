using Scripts.Containers;
using Scripts.Containers.Entity;
using Scripts.Containers.Service;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameInitializer : MonoBehaviour
    {
        private IGameService gameService;
        private IEntityContainer entityContainer;

        private void Awake()
        {
            gameService = ServiceContainer.GameService;
            entityContainer = GameContainers.EntityContainer;
        }
    }
}