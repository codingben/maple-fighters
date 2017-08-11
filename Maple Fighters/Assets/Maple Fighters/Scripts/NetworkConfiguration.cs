using ExitGames.Client.Photon;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NetworkConfiguration", menuName = "Maple Fighters/Scriptable Objects/NetworkConfiguration", order = 1)]
    public class NetworkConfiguration : ScriptableObject
    {
        public ConnectionProtocol ConnectionProtocol;
        public DebugLevel DebugLevel;
    }
}