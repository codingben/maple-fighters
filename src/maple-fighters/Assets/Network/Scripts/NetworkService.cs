using System.Threading.Tasks;
using ClientCommunicationInterfaces;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using ExitGames.Client.Photon;
using UnityEngine;

namespace Network.Scripts
{
    public abstract class NetworkService : MonoBehaviour
    {
        public async Task<ConnectionStatus> ConnectAsync(IYield yield)
        {
            var connectionStatus = ConnectionStatus.Failed;

            try
            {
                var connector = GetServerConnector();
                var info = GetConnectionInfo();
                var protocol = GetConnectionProtocol();
                var serverPeer = await connector.Connect(yield, info, protocol);

                OnConnected(serverPeer);

                connectionStatus = ConnectionStatus.Succeed;
            }
            catch (CouldNotConnectToPeerException exception)
            {
                var info = GetConnectionInfo();
                var server = $"{info.Ip}:{info.Port}";
                var reason = exception.Message;

                Debug.Log($"Could not connect to {server}. Reason: {reason}");
            }

            return connectionStatus;
        }

        protected abstract void OnConnected(IServerPeer serverPeer);

        protected abstract IServerConnector GetServerConnector();

        protected abstract PeerConnectionInformation GetConnectionInfo();

        protected abstract ConnectionProtocol GetConnectionProtocol();
    }
}