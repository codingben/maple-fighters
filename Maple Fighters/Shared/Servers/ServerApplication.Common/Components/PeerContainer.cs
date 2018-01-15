using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class PeerContainer : Component<IServerEntity>, IPeerContainer
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, IClientPeerWrapper<IClientPeer>> peerLogics = new Dictionary<int, IClientPeerWrapper<IClientPeer>>();

        public void AddPeerLogic(IClientPeerWrapper<IClientPeer> peerLogic)
        {
            lock (locker)
            {
                if (peerLogics.ContainsKey(peerLogic.PeerId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A peer id {peerLogic.PeerId} has been already registered."));
                    return;
                }

                peerLogic.Peer.PeerDisconnectionNotifier.Disconnected += (reason, details) => RemovePeerLogic(peerLogic, reason, details);
                peerLogics.Add(peerLogic.PeerId, peerLogic);
            }
        }

        private void RemovePeerLogic(IClientPeerWrapper<IClientPeer> peerLogic, DisconnectReason reason, string details)
        {
            lock (locker)
            {
                var ip = peerLogic.Peer.ConnectionInformation.Ip;
                var port = peerLogic.Peer.ConnectionInformation.Port;

                LogUtils.Log(details == string.Empty
                    ? $"A peer {ip}:{port} has been disconnected. Reason: {reason}"
                    : $"A peer {ip}:{port} has been disconnected. Reason: {reason} Details: {details}");

                peerLogics.Remove(peerLogic.PeerId);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            DisconnectAllPeers();
        }

        private void DisconnectAllPeers()
        {
            lock (locker)
            {
                foreach (var peer in peerLogics)
                {
                    peer.Value.Peer.Disconnect();
                }

                peerLogics.Clear();
            }
        }

        public IClientPeerWrapper<IClientPeer> GetPeerWrapper(int peerId)
        {
            lock (locker)
            {
                return peerLogics.TryGetValue(peerId, out var peerWrapper) ? peerWrapper : null;
            }
        }

        public IEnumerable<IClientPeerWrapper<IClientPeer>> GetAllPeerWrappers()
        {
            lock (locker)
            {
                return peerLogics.Values.ToArray();
            }
        }

        public int GetPeersCount()
        {
            lock (locker)
            {
                return peerLogics.Count;
            }
        }
    }
}