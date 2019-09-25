using CommonCommunicationInterfaces;
using Scripts.Containers;
using Scripts.Network.Core;
using UnityEngine;

namespace Scripts.World
{
    public class NetworkTrafficController : MonoBehaviour
    {
        private void Awake()
        {
            if (ServiceContainer.GameService is IServiceBase service)
            {
                service.SetNetworkTrafficState(NetworkTrafficState.Flowing);
            }
        }

        private void OnDestroy()
        {
            if (ServiceContainer.GameService is IServiceBase service)
            {
                service.SetNetworkTrafficState(NetworkTrafficState.Paused);
            }
        }
    }
}