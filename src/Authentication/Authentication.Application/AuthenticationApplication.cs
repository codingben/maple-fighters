using CommonTools.Log;
using ServerCommon.Application;
using ServerCommunicationInterfaces;

namespace Authentication.Application
{
    public class AuthenticationApplication : ServerApplicationBase
    {
        public AuthenticationApplication(IFiberProvider fiberProvider)
            : base(fiberProvider)
        {
            // Left blank intentionally
        }

        protected override void OnStartup()
        {
            base.OnStartup();

            LogUtils.Log("OnStartup");
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            LogUtils.Log("OnShutdown");
        }
    }
}