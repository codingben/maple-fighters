using CommonTools.Log;
using ServerCommon.Application;
using ServerCommon.Configuration;
using ServerCommunicationInterfaces;

namespace Authentication.Application
{
    public class AuthenticationApplication : ServerApplicationBase
    {
        public AuthenticationApplication(IFiberProvider fiberProvider)
            : base(fiberProvider)
        {
            ServerSettings.InboundPeer.LogEvents = true;
            ServerSettings.InboundPeer.Operations.LogRequests = true;
            ServerSettings.InboundPeer.Operations.LogResponses = true;
            ServerSettings.OutboundPeer.LogEvents = true;
            ServerSettings.OutboundPeer.Operations.LogRequests = true;
            ServerSettings.OutboundPeer.Operations.LogResponses = true;
        }

        protected override void OnStartup()
        {
            base.OnStartup();

            AddCommonComponents();

            LogUtils.Log("OnStartup");
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            LogUtils.Log("OnShutdown");
        }
    }
}