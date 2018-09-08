using Common.ComponentModel;
using ServerCommon.Application.Components;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    public class ServerApplicationBase : IApplicationBase
    {
        protected IComponentsProvider Components
        {
            get
            {
                if (components == null)
                {
                    throw new ServerApplicationException(
                        "Components should be initialized from OnStartup method!");
                }

                return components;
            }

            set
            {
                if (components == null)
                {
                    components = value;
                }
            }
        }

        private IComponentsProvider components;

        public void Startup()
        {
            OnStartup();
        }

        public void Shutdown()
        {
            OnShutdown();
        }

        public void Connected(IClientPeer clientPeer)
        {
            OnConnected(clientPeer);
        }

        protected virtual void OnStartup()
        {
            Components = new ComponentsProvider();
        }

        protected virtual void OnShutdown()
        {
            Components.Dispose();
        }

        protected virtual void OnConnected(IClientPeer clientPeer)
        {
            // TODO: Implement
        }

        private void AddCommonComponents()
        {
            Components.Add(new IdGenerator());
            Components.Add(new RandomNumberGenerator());
        }
    }
}