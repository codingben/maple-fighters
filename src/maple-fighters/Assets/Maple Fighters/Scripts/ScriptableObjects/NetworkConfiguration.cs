using System;
using CommonCommunicationInterfaces;
using ExitGames.Client.Photon;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        fileName = "NetworkConfiguration",
        menuName = "Scriptable Objects/NetworkConfiguration",
        order = 0)]
    public class NetworkConfiguration : ScriptableSingleton<NetworkConfiguration>
    {
        public ConnectionProtocol ConnectionProtocol = ConnectionProtocol.Udp;
        public DebugLevel DebugLevel = DebugLevel.OFF;
        public bool LogOperationsRequest;
        public bool LogOperationsResponse;
        public bool LogEvents;

        public PeerConnectionInformation GetPeerConnectionInformation(
            ConnectionInformation connectionInformation)
        {
            PeerConnectionInformation peerConnectionInformation;

            if (ConnectionProtocol == ConnectionProtocol.Tcp)
            {
                Debug.LogWarning(
                    "TCP is not supported. TCP is only for communication between servers.");
            }

            switch (ConnectionProtocol)
            {
                case ConnectionProtocol.Udp:
                case ConnectionProtocol.Tcp:
                {
                    peerConnectionInformation =
                        connectionInformation.UdpConnectionDetails;
                    break;
                }

                case ConnectionProtocol.WebSocket:
                {
                    peerConnectionInformation = connectionInformation
                        .WebSocketConnectionDetails;
                    break;
                }

                case ConnectionProtocol.WebSocketSecure:
                {
                    peerConnectionInformation = connectionInformation
                        .WebSocketSecureConnectionDetails;
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