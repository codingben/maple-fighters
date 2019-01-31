namespace Scripts.Services
{
    public class PeerLogicBase : IPeerLogicBase
    {
        protected IServerPeerHandler ServerPeerHandler { get; private set; }

        public void Awake(IServerPeerHandler serverPeerHandler)
        {
            ServerPeerHandler = serverPeerHandler;

            OnAwake();
        }

        protected virtual void OnAwake()
        {
            // Left blank intentionally
        }

        public void Dispose()
        {
            OnDestroy();
        }

        protected virtual void OnDestroy()
        {
            // Left blank intentionally
        }
    }
}