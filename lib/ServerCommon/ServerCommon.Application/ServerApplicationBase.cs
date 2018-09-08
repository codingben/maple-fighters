using Common.ComponentModel;
using ServerCommon.Application.Components;

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
                        "Components should be initialized from OnStartup().");
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

        protected virtual void OnStartup()
        {
            Components = new ComponentsProvider();
        }

        protected virtual void OnShutdown()
        {
            Components.Dispose();
        }

        private void AddCommonComponents()
        {
            Components.Add(new IdGenerator());
            Components.Add(new RandomNumberGenerator());
        }
    }
}