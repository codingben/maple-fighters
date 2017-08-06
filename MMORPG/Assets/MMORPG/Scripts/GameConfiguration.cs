using ExitGames.Client.Photon;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfiguration", menuName = "Scriptable Objects/Game Configuration", order = 1)]
    public class GameConfiguration : ScriptableObject
    {
        public ConnectionProtocol ConnectionProtocol = ConnectionProtocol.Udp;
    }
}