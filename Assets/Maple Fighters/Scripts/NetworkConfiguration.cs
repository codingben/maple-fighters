using ExitGames.Client.Photon;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NetworkConfiguration", menuName = "Scriptable Objects/NetworkConfiguration", order = 0)]
    public class NetworkConfiguration : ScriptableObjectSingleton<NetworkConfiguration>
    {
        public ConnectionProtocol ConnectionProtocol = ConnectionProtocol.Udp;
        public DebugLevel DebugLevel = DebugLevel.OFF;
        public bool LogOperationsRequest;
        public bool LogOperationsResponse;
        public bool LogEvents;
    }
}