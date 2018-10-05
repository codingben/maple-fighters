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
        protected IComponentsProvider Components { get; }

        protected IExposedComponentsProvider ExposedComponents { get; }

        private readonly IFiberProvider fiberProvider;

        protected internal ServerApplicationBase(IFiberProvider fiberProvider)
        {
            this.fiberProvider = fiberProvider;

            LogUtils.Logger = new Logger();
            TimeProviders.DefaultTimeProvider = new TimeProvider();

            Components = new ComponentsProvider();
            ExposedComponents = Components.ProvideExposed();

            ServerExposedComponents.SetProvider(ExposedComponents);
            ServerConfiguration.Setup();
        }

        void IApplicationBase.Startup()
        {
            OnStartup();
        }

        void IApplicationBase.Shutdown()
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
        /// 1. <see cref="IIdGenerator"/>
        /// 2. <see cref="IRandomNumberGenerator"/>
        /// 3. <see cref="IFiberStarter"/>
        /// 4. <see cref="ICoroutinesExecutor"/>
        /// 5. <see cref="IPeersLogicsProvider"/>
        /// </summary>
        protected void AddCommonComponents()
        {
            ExposedComponents.Add(new IdGenerator());
            Components.Add(new RandomNumberGenerator());

            IFiberStarter fiber =
                Components.Add(new FiberStarter(fiberProvider));

            Components.Add(
                new CoroutinesExecutor(
                    new FiberCoroutinesExecutor(
                        fiber.GetFiberStarter(),
                        updateRateMilliseconds: 100)));
            ExposedComponents.Add(new PeersLogicsProvider());
        }
    }
}