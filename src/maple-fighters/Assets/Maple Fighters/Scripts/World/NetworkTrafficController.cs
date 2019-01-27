using CommonCommunicationInterfaces;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.World
{
    public class NetworkTrafficController : MonoBehaviour
    {
        private void Awake()
        {
            ServiceContainer.GameService.ServiceConnectionHandler
                .SetNetworkTrafficState(NetworkTrafficState.Flowing);
        }

        private void OnDestroy()
        {
            ServiceContainer.GameService.ServiceConnectionHandler
                .SetNetworkTrafficState(NetworkTrafficState.Paused);
        }
    }
}