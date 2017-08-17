using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;
using Shared.Communication.Common.Peer;

namespace ServerApplication.Common.Components
{
    public class PeerContainer : IComponent
    {
        private readonly Dictionary<int, ClientPeer<IClientPeer>> peerLogics = new Dictionary<int, ClientPeer<IClientPeer>>();

        public void Dispose()
        {
            foreach (var peer in peerLogics)
            {
                peer.Value.Dispose();
            }

            peerLogics.Clear();
        }

        public void AddPeerLogic(int peerId, ClientPeer<IClientPeer> peerLogic)
        {
            peerLogic.Disconnected += (reason, details) => NotifyAndRemovePeerLogic(peerId, reason, details);

            peerLogics.Add(peerId, peerLogic);
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
    }
}