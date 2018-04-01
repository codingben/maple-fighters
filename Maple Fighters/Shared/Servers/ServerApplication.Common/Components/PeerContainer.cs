using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ComponentModel.Common;
using PeerLogic.Common;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class PeerContainer : Component, IPeerContainer
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, IClientPeerWrapper> peerLogics = new Dictionary<int, IClientPeerWrapper>();

        public void AddPeerLogic(IClientPeerWrapper peerLogic)
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

        private void RemovePeerLogic(IClientPeerWrapper peerLogic, DisconnectReason reason, string details)
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

        public IClientPeerWrapper GetPeerWrapper(int peerId)
        {
            lock (locker)
            {
                return peerLogics.TryGetValue(peerId, out var peerWrapper) ? peerWrapper : null;
            }
        }

        public IEnumerable<IClientPeerWrapper> GetAllPeerWrappers()
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