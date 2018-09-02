using Common.ComponentModel;
using ServerCommon.Application.Components;

namespace ServerCommon.Application
{
    public class ApplicationBase : IApplicationBase
    {
        protected IComponentsProvider Components =>
            ServerComponentsProvider.GetComponentsProvider();

        protected IServerComponentsProvider ServerComponentsProvider { get; }

        public ApplicationBase()
        {
            ServerComponentsProvider = new ServerComponentsProvider();
        }

        public void Startup()
        {
            OnStartup();
        }

        public void Shutdown()
        {
            OnShutdown();
        }

        protected virtual void OnStartup()
        {
            // TODO: Implement
        }

        protected virtual void OnShutdown()
        {
            // TODO: Implement
        }
    }
}