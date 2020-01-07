using Common.ComponentModel;
using Common.Components;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommon.Application.Components;
using ServerCommon.Communication.Components;
using ServerCommon.Configuration;
using ServerCommon.Logging;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application
{
    /// <inheritdoc />
    /// <summary>
    /// A base for the server application to be initialized properly.
    /// </summary>
    public class ServerApplicationBase : IApplicationBase
    {
        protected IExposedComponents ExposedComponents =>
            Components.ProvideExposed();

        protected IComponents Components => new ComponentsProvider();

        private readonly IServerConnector serverConnector;
        private readonly IFiberProvider fiberProvider;

        protected ServerApplicationBase(IServerConnector serverConnector, IFiberProvider fiberProvider)
        {
            this.serverConnector = serverConnector;
            this.fiberProvider = fiberProvider;

            LogUtils.Logger = new Logger();
            TimeProviders.DefaultTimeProvider = new TimeProvider();

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
        /// 5. <see cref="IS2sConnectionProvider"/>
        /// </summary>
        protected void AddCommonComponents()
        {
            ExposedComponents.Add(new IdGenerator());
            Components.Add(new RandomNumberGenerator());

            IFiberStarter fiber =
                Components.Add(new FiberStarter(fiberProvider));
            var scheduler = fiber.GetFiberStarter();
            var executor = new FiberCoroutinesExecutor(scheduler, updateRateMilliseconds: 100);

            Components.Add(new CoroutinesExecutor(executor));
            Components.Add(new S2sConnectionProvider(serverConnector));
        }
    }
}