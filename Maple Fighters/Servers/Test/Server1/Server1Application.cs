using CommonCommunicationInterfaces;
using CommonTools.Log;
using Server2.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace Server1
{
    public class Server1Application : ApplicationBase
    {
        public Server1Application(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }

        public override void Startup()
        {
            base.Startup();

            AddCommonComponents();
            AddS2SComponents();
        }

        private void AddS2SComponents()
        {
            var server2Service = Server.Components.AddComponent(new Server2Service());
            server2Service.TestEvent += OnTestEvent;
        }

        private void OnTestEvent(EmptyParameters emptyParameters)
        {
            LogUtils.Log(MessageBuilder.Trace("Hello world!"));
        }
    }
}