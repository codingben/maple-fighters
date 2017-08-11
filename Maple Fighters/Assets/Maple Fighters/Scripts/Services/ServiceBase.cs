using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using PhotonClientImplementation;
using Scripts.ScriptableObjects;
using Scripts.Utils;
using UnityEngine;
using PhotonPeer = PhotonClientImplementation.PhotonPeer;

namespace Scripts.Services
{
    public abstract class ServiceBase : MonoBehaviour, ICoroutinesExecuter
    {
        public int Count => coroutinesExecuter.Count;

        private ExternalCoroutinesExecuter coroutinesExecuter;
        [SerializeField] private NetworkConfiguration networkConfiguration;

        protected PhotonPeer PhotonPeer;

        [SerializeField] private ConnectionInformation connectionInformation;

        private ServersType currentServerType;
        private PeerConnectionInformation currentConnectionInformation;

        private void Awake()
        {
            coroutinesExecuter = new ExternalCoroutinesExecuter();

            Initiate();
        }

        private void Update()
        {
            coroutinesExecuter.Update();
        }

        private void OnApplicationQuit()
        {
            PhotonPeer?.Disconnect();
        }

        protected abstract void Initiate();

        protected void Connect()
        {
            InitializePhotonPeer();

            if (PhotonPeer == null)
            {
                return;
            }

            Debug.Log($"ServiceBase::Connect() -> Connecting to {currentServerType} - " +
                      $"{currentConnectionInformation.Ip}:{currentConnectionInformation.Port}");

            PhotonPeer.Connect();

            coroutinesExecuter.StartCoroutine(PhotonPeer.WaitForConnect(OnConnected, OnConnectionFailed));
        }

        private void InitializePhotonPeer()
        {
            switch (networkConfiguration.ConnectionProtocol)
            {
                case ConnectionProtocol.Udp:
                    currentServerType = connectionInformation.ServerType;
                    currentConnectionInformation = connectionInformation.UdpConnectionDetails;
                    break;
                case ConnectionProtocol.Tcp:
                    currentServerType = connectionInformation.ServerType;
                    currentConnectionInformation = connectionInformation.TcpConnectionDetails;
                    break;
                case ConnectionProtocol.WebSocket:
                case ConnectionProtocol.WebSocketSecure:
                    currentServerType = connectionInformation.ServerType;
                    currentConnectionInformation = connectionInformation.WebConnectionDetails;
                    break;
            }

            if (networkConfiguration.ConnectionProtocol == ConnectionProtocol.WebSocketSecure)
            {
                Debug.LogError($"Connection type {networkConfiguration} is not supported yet.");
                return;
            }

            PhotonPeer = new PhotonPeer(currentConnectionInformation, networkConfiguration.ConnectionProtocol, networkConfiguration.DebugLevel, 
                coroutinesExecuter);
        }

        protected void OnConnectionFailed()
        {
            Debug.LogWarning("ServiceBase::OnConnectionFailed() -> The connection was not established. Connection details: " +
                             $"{currentServerType} - {currentConnectionInformation.Ip}:{currentConnectionInformation.Port}");
        }

        protected virtual void OnConnected()
        {
            Debug.Log("ServiceBase::OnConnected() -> The connection was established successfully. Connection details: " +
                      $"{currentServerType} - {currentConnectionInformation.Ip}:{currentConnectionInformation.Port}");

            PhotonPeer.Disconnected += OnDisconnected;
        }

        protected void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            Debug.Log("ServiceBase::OnDisconnected() -> The connection has been closed. Connection details: " +
                      $"{currentServerType} - {currentConnectionInformation.Ip}:{currentConnectionInformation.Port}. Reason: {disconnectReason}");

            PhotonPeer.Disconnected -= OnDisconnected;
        }

        public void Dispose()
        {
            PhotonPeer?.Disconnect();
        }

        public ICoroutine StartCoroutine(IEnumerator<IYieldInstruction> coroutineEnumerator)
        {
            return coroutinesExecuter.StartCoroutine(coroutineEnumerator);
        }
    }
}