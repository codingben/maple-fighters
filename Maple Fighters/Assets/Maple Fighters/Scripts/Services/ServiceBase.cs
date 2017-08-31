using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommunicationHelper;
using ExitGames.Client.Photon;
using PhotonClientImplementation;
using Scripts.Coroutines;
using Scripts.ScriptableObjects;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.Services
{
    public abstract class ServiceBase<TOperationCode, TEventCode> : IDisposable
        where TOperationCode : IComparable, IFormattable, IConvertible
        where TEventCode : IComparable, IFormattable, IConvertible
    {
        private ServersType serverType;
        private PeerConnectionInformation peerConnectionInformation;

        private IServerPeer serverPeer;

        protected IEventHandlerRegister<TEventCode> EventHandlerRegister { get; private set; }
        protected IOperationRequestSender<TOperationCode> OperationRequestSender { get; private set; }
        protected IOperationResponseSubscriptionProvider SubscriptionProvider { get; private set; }

        protected readonly ExternalCoroutinesExecutor CoroutinesExecuter;

        protected ServiceBase()
        {
            Debug.Log("ServiceBase");

            CoroutinesExecuter = new ExternalCoroutinesExecutor().ExecuteExternally();
        }

        public async Task<IServerPeer> ConnectAsync(IYield yield, PeerConnectionInformation connectionInformation)
        {
            var serverConnector = new PhotonServerConnector(() => CoroutinesExecuter);
            var networkConfiguration = NetworkConfiguration.GetInstance();

            serverPeer = await serverConnector.ConnectAsync(yield, connectionInformation,
                new ConnectionDetails(networkConfiguration.ConnectionProtocol, networkConfiguration.DebugLevel));

            if (serverPeer == null)
            {
                return null;
            }

            InitializePeerHandlers();
            OnConnected();

            return serverPeer;
        }

        public void Dispose()
        {
            CoroutinesExecuter.RemoveFromExternalExecuter().Dispose();

            serverPeer?.Disconnect();

            SubscriptionProvider?.Dispose();
            EventHandlerRegister?.Dispose();
        }

        protected void Connect(ConnectionInformation connectionInformation)
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();

            serverType = connectionInformation.ServerType;

            switch (networkConfiguration.ConnectionProtocol)
            {
                case ConnectionProtocol.Udp:
                    peerConnectionInformation = connectionInformation.UdpConnectionDetails;
                    break;
                case ConnectionProtocol.Tcp:
                    peerConnectionInformation = connectionInformation.TcpConnectionDetails;
                    break;
                case ConnectionProtocol.WebSocket:
                case ConnectionProtocol.WebSocketSecure:
                    peerConnectionInformation = connectionInformation.WebConnectionDetails;
                    break;
            }

            Debug.Log($"Connecting to a {serverType} server - " + $"{peerConnectionInformation.Ip}:{peerConnectionInformation.Port}");

            CoroutinesExecuter.StartTask(y => ConnectAsync(y, peerConnectionInformation));
        }

        protected abstract void OnConnected();

        protected abstract void OnDisconnected();

        private void OnDisconnected(DisconnectReason disconnectReason, string s)
        {
            serverPeer.PeerDisconnectionNotifier.Disconnected -= OnDisconnected;

            Debug.Log("A connection has been closed with " +
                      $"{serverType} - {peerConnectionInformation.Ip}:{peerConnectionInformation.Port}. Reason: {disconnectReason}");

            OnDisconnected();
        }

        private void InitializePeerHandlers()
        {
            SubscriptionProvider = new OperationResponseSubscriptionProvider<TOperationCode>(serverPeer.OperationResponseNotifier,
                (data, s) => Debug.LogError($"Sending an operaiton has been failed. Operation Code: {data.Code} - Server Type: {serverType}"));
            EventHandlerRegister = new EventHandlerRegister<TEventCode>(serverPeer.EventNotifier);
            OperationRequestSender = new OperationRequestSender<TOperationCode>(serverPeer.OperationRequestSender);

            serverPeer.PeerDisconnectionNotifier.Disconnected += OnDisconnected;
        }

        private void OnApplicationQuit()
        {
            Dispose();
        }

        protected bool IsConnected()
        {
            return serverPeer.IsConnected;
        }
    }
}