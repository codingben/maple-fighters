using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.ServerApplication.Common.Peer;

namespace ServerApplication.Common.Components
{
    public class PeerContainer : IComponent
    {
        private readonly Dictionary<int, IClientPeerWrapper<IClientPeer>> peerLogics = new Dictionary<int, IClientPeerWrapper<IClientPeer>>();

        public void Dispose()
        {
            foreach (var peer in peerLogics)
            {
                peer.Value.Dispose();
            }

            peerLogics.Clear();
        }

        public void AddPeerLogic(IClientPeerWrapper<IClientPeer> peerLogic)
        {
            var peerId = peerLogic.PeerId;

            SubscribeToPeerDisconnectionNotifier(peerLogic, peerId);

            peerLogics.Add(peerId, peerLogic);

            LogUtils.Log($"A new peer has been created -> {peerLogic.Peer.ConnectionInformation.Ip}:{peerLogic.Peer.ConnectionInformation.Port}");
        }

        private void SubscribeToPeerDisconnectionNotifier(IClientPeerWrapper<IClientPeer> peerLogic, int peerId)
        {
            peerLogic.Disconnected += (reason, details) => NotifyAndRemovePeerLogic(peerId, reason, details);
        }

        private void NotifyAndRemovePeerLogic(int peerId, DisconnectReason reason, string details)
        {
            var peerLogic = peerLogics[peerId];

            var ip = peerLogic.Peer.ConnectionInformation.Ip;
            var port = peerLogic.Peer.ConnectionInformation.Port;

            LogUtils.Log(details == string.Empty ? $"A peer {ip}:{port} has been disconnected. Reason: {reason}"
                : $"A peer {ip}:{port} has been disconnected. Reason: {reason} Details: {details}");

            peerLogics.Remove(peerId);
        }

        public int GetPeersCount()
        {
            return peerLogics.Count;
        }
    }
}