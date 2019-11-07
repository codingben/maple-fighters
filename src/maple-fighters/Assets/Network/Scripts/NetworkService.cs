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
        public async Task ConnectAsync(IYield yield)
        {
            try
            {
                var connector = GetServerConnector();
                var info = GetConnectionInfo();
                var protocol = GetConnectionProtocol();
                var serverPeer = await connector.Connect(yield, info, protocol);

                OnConnected(serverPeer);
            }
            catch (CouldNotConnectToPeerException exception)
            {
                var info = GetConnectionInfo();
                var server = $"{info.Ip}:{info.Port}";
                var reason = exception.Message;

                Debug.Log($"Could not connect to {server}. Reason: {reason}");
            }
        }

        protected abstract void OnConnected(IServerPeer serverPeer);

        protected abstract IServerConnector GetServerConnector();

        protected abstract PeerConnectionInformation GetConnectionInfo();

        protected abstract ConnectionProtocol GetConnectionProtocol();
    }
}