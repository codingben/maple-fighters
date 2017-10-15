using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;

namespace Database.Common.AccessToken
{
    public class LocalDatabaseAccessTokens : Component<IServerEntity>
    {
        private readonly Dictionary<int, string> accessTokens = new Dictionary<int, string>();
        private readonly object locker = new object();

        private PeerContainer peerContainer;

        protected override void OnAwake()
        {
            base.OnAwake();

            peerContainer = Entity.Container.GetComponent<PeerContainer>().AssertNotNull();
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

                peerContainer.GetPeerWrapper(peerId).Disconnected += (reason, s) => Remove(peerId);
            }
        }

        public void Remove(int peerId)
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