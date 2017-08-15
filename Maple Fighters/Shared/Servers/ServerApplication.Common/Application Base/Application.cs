using System.Collections.Generic;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using ServerCommunicationInterfaces;
using Shared.Communication.Common.Peer;

namespace ServerApplication.Common.ApplicationBase
{
    /// <summary>
    /// Every server application should inherit from this base class to initialize a server application.
    /// </summary>
    public abstract class Application
    {
        private readonly List<ClientPeer<IClientPeer>> peerLogics = new List<ClientPeer<IClientPeer>>();

        public abstract void OnConnected(IClientPeer clientPeer);

        public abstract void Initialize();

        public void Dispose()
        {
            peerLogics.ForEach(x => x.Dispose());
            peerLogics.Clear();
        }

        protected void AddCommonComponents()
        {
            ServerComponents.Container.AddComponent(new RandomNumberGenerator());
        }

        protected void AddPeerLogic(ClientPeer<IClientPeer> peerLogic)
        {
            peerLogic.Disconnected += (reason, details) => RemovePeerLogic(peerLogic, reason, details);

            peerLogics.Add(peerLogic);
        }

        private void RemovePeerLogic(ClientPeer<IClientPeer> peerLogic, DisconnectReason reason, string details)
        {
            var ip = peerLogic.Peer.ConnectionInformation.Ip;
            var port = peerLogic.Peer.ConnectionInformation.Port;

            LogUtils.Log(details == string.Empty ? $"A peer {ip}:{port} has been disconnected. Reason: {reason}"
                : $"A peer {ip}:{port} has been disconnected. Reason: {reason} Details: {details}");

            peerLogics.Remove(peerLogic);
        }
    }
}