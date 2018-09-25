using Common.ComponentModel;
using Common.Components;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.Configuration;
using ServerCommon.Logging;
using ServerCommon.PeerLogic.Components;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    /// <inheritdoc />
    /// <summary>
    /// A base for the server application to be initialized properly.
    /// </summary>
    public class ServerApplicationBase : IApplicationBase
    {
        protected IComponentsProvider Components => new ComponentsProvider();

        private readonly IFiberProvider fiberProvider;

        protected internal ServerApplicationBase(IFiberProvider fiberProvider)
        {
            this.fiberProvider = fiberProvider;

            LogUtils.Logger = new Logger();
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            ServerExposedComponents.SetProvider(Components.ProvideExposed());
            ServerConfiguration.Setup();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IApplicationBase.Startup"/> for more information.
        /// </summary>
        public void Startup()
        {
            OnStartup();
        }

        /// <inheritdoc />
        /// <summary>
        /// See <see cref="IApplicationBase.Shutdown"/> for more information.
        /// </summary>
        public void Shutdown()
        {
            OnShutdown();
        }

        protected virtual void OnStartup()
        {
            LogUtils.Log("An application has started.");
        }

        protected virtual void OnShutdown()
        {
            Components?.Dispose();

            LogUtils.Log("An application has been stopped.");
        }

        /// <summary>
        /// Adds common components:
        /// IdGenerator
        /// RandomNumberGenerator
        /// FiberStarter
        /// CoroutinesManager
        /// PeersLogicProvider
        /// </summary>
        protected void AddCommonComponents()
        {
            Components.Add(new IdGenerator());
            Components.Add(new RandomNumberGenerator());
            IFiberStarter fiber = Components.Add(new FiberStarter(fiberProvider));
            var executor = new FiberCoroutinesExecutor(
                fiber.GetFiberStarter(), updateRateMilliseconds: 100);
            Components.Add(new CoroutinesManager(executor));
            Components.Add(new PeersLogicsProvider());
        }
    }
}