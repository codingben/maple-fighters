using System;
using CommonCommunicationInterfaces;
using ExitGames.Client.Photon;
using Scripts.Utils;
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

        public PeerConnectionInformation GetPeerConnectionInformation(ConnectionInformation connectionInformation)
        {
            PeerConnectionInformation peerConnectionInformation;

            switch (ConnectionProtocol)
            {
                case ConnectionProtocol.Udp:
                {
                    peerConnectionInformation = connectionInformation.UdpConnectionDetails;
                    break;
                }
                case ConnectionProtocol.Tcp:
                {
                    peerConnectionInformation = connectionInformation.TcpConnectionDetails;
                    break;
                }
                case ConnectionProtocol.WebSocket:
                case ConnectionProtocol.WebSocketSecure:
                {
                    peerConnectionInformation = connectionInformation.WebConnectionDetails;
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return peerConnectionInformation;
        }
    }
}