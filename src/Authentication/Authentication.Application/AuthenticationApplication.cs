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
            ServerSettings.Peer.LogEvents = true;
            ServerSettings.Peer.Operations.LogRequests = true;
            ServerSettings.Peer.Operations.LogResponses = true;
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