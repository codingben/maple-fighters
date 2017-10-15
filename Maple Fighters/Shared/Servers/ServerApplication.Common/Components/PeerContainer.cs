using System.Collections.Generic;
using System.Linq;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.PeerLogic;

namespace ServerApplication.Common.Components
{
    public class PeerContainer : Component<IServerEntity>
    {
        private readonly Dictionary<int, IClientPeerWrapper<IClientPeer>> peerLogics = new Dictionary<int, IClientPeerWrapper<IClientPeer>>();
        private readonly object locker = new object();

        public void AddPeerLogic(IClientPeerWrapper<IClientPeer> peerLogic)
        {
            peerLogics.Add(peerLogic.PeerId, peerLogic);

            peerLogic.Disconnected += (reason, details) => RemovePeerLogic(peerLogic, reason, details);
        }

        public void DisconnectAllPeers()
        {
            foreach (var peer in peerLogics)
            {
                peer.Value.Dispose();
            }

            peerLogics.Clear();
        }
        
        private void RemovePeerLogic(IClientPeerWrapper<IClientPeer> peerLogic, DisconnectReason reason, string details)
        {
            var ip = peerLogic.Peer.ConnectionInformation.Ip;
            var port = peerLogic.Peer.ConnectionInformation.Port;

            LogUtils.Log(details == string.Empty ? $"A peer {ip}:{port} has been disconnected. Reason: {reason}"
                : $"A peer {ip}:{port} has been disconnected. Reason: {reason} Details: {details}");

            peerLogics.Remove(peerLogic.PeerId);
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
            return peerLogics.Count;
        }
    }
}