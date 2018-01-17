using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ComponentModel.Common;
using ServerApplication.Common.Components;

namespace Database.Common.AccessToken
{
    public class LocalDatabaseAccessTokens : Component, ILocalDatabaseAccessTokens
    {
        private readonly Dictionary<int, string> accessTokens = new Dictionary<int, string>();
        private readonly object locker = new object();

        private IPeerContainer peerContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Entity.GetComponent<IPeerContainer>().AssertNotNull();
        }

        public void Add(int peerId, string accessToken)
        {
            lock (locker)
            {
                if (accessTokens.ContainsKey(peerId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A peer with id #{peerId} already exists in a database of an access tokens."));
                    return;
                }

                accessTokens.Add(peerId, accessToken);

                SubscribeToPeerDisconnectionNotifier(peerId);
            }
        }

        private void SubscribeToPeerDisconnectionNotifier(int peerId)
        {
            peerContainer.GetPeerWrapper(peerId).Peer.PeerDisconnectionNotifier.Disconnected += (reason, s) => Remove(peerId);
        }

        private void Remove(int peerId)
        {
            lock (locker)
            {
                if (!accessTokens.ContainsKey(peerId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"A peer with id #{peerId} does not exist in a database of an access tokens."));
                    return;
                }

                accessTokens.Remove(peerId);
            }
        }

        public bool Exists(string accessToken)
        {
            lock (locker)
            {
                return accessTokens.Values.Any(token => token == accessToken);
            }
        }
    }
}