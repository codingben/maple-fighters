using CommonCommunicationInterfaces;
using Scripts.Network.Core;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.Network
{
    public class NetworkTrafficStateSetter : MonoBehaviour
    {
        private void Awake()
        {
            if (ServiceProvider.GameService is IServiceBase service)
            {
                service.SetNetworkTrafficState(NetworkTrafficState.Flowing);
            }
        }

        private void OnDestroy()
        {
            if (ServiceProvider.GameService is IServiceBase service)
            {
                service.SetNetworkTrafficState(NetworkTrafficState.Paused);
            }
        }
    }
}