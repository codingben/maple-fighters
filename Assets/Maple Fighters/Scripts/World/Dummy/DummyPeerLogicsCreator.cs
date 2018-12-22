using Game.Common;
using Scripts.Containers;
using Scripts.Services;
using UnityEngine;

namespace Assets.Scripts
{
    public class DummyPeerLogicsCreator : MonoBehaviour
    {
        private void Awake()
        {
            if (ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>() == null)
            {
                ServiceContainer.GameService
                    .SetPeerLogic<DummyGameScenePeerLogic, GameOperations, GameEvents>();
            }
        }
    }
}