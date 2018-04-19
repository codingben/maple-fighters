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
            ServiceContainer.GameService.SetPeerLogic<DummyGameScenePeerLogic, GameOperations, GameEvents>();
        }
    }
}