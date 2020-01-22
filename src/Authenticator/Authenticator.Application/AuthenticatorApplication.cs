using Authenticator.Infrastructure.Repository;
using Common.MongoDB;
using CommonTools.Coroutines;
using CommonTools.Log;
using ServerCommon.Application;
using ServerCommon.Configuration;
using ServerCommon.Logging;
using ServerCommunicationInterfaces;

namespace Authenticator.Application
{
    public class AuthenticatorApplication : ServerApplicationBase
    {
        public AuthenticatorApplication(IServerConnector serverConnector, IFiberProvider fiberProvider)
            : base(serverConnector, fiberProvider)
        {
            // Initializing server settings
            ServerConfiguration.Setup();

            // Setting server settings
            ServerSettings.InboundPeer.LogEvents = true;
            ServerSettings.InboundPeer.Operations.LogRequests = true;
            ServerSettings.InboundPeer.Operations.LogResponses = true;
            ServerSettings.OutboundPeer.LogEvents = true;
            ServerSettings.OutboundPeer.Operations.LogRequests = true;
            ServerSettings.OutboundPeer.Operations.LogResponses = true;
            ServerSettings.Databases.Mongo.Url = "mongodb://localhost:27017/maple_fighters";
        }

        protected override void OnStartup()
        {
            base.OnStartup();

            AddCommonComponents();
            AddComponents();

            LogUtils.Log("OnStartup");
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            LogUtils.Log("OnShutdown");
        }

        protected override ILogger GetLogger()
        {
            return new Logger();
        }

        protected override ITimeProvider GetTimeProvider()
        {
            return new TimeProvider();
        }

        private void AddComponents()
        {
            var url = ServerSettings.Databases.Mongo.Url.AssertNotNull();
            Components.Add(new MongoDatabaseProvider(url));
            Components.Add(new AccountRepository());
        }
    }
}