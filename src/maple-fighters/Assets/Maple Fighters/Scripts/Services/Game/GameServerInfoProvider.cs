using CommonCommunicationInterfaces;
using UnityEngine;

namespace Scripts.Services.Game
{
    public class GameServerInfoProvider : MonoBehaviour
    {
        private PeerConnectionInformation connectionInformation;

        private void Awake()
        {
            connectionInformation = new PeerConnectionInformation();

            DontDestroyOnLoad(gameObject);
        }

        public void SetConnectionInfo(string ip, int port)
        {
            connectionInformation.Ip = ip;
            connectionInformation.Port = port;
        }

        public PeerConnectionInformation GetConnectionInfo()
        {
            return connectionInformation;
        }
    }
}