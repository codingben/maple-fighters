using CommonCommunicationInterfaces;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.World
{
    public class NetworkTrafficController : MonoBehaviour
    {
        private void Awake()
        {
            ServiceContainer.GameService.SetNetworkTrafficState(
                NetworkTrafficState.Flowing);
        }

        private void OnDestroy()
        {
            ServiceContainer.GameService.SetNetworkTrafficState(
                NetworkTrafficState.Paused);
        }
    }
}