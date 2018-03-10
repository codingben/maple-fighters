using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace GameServerProvider.Service.Application
{
    public class GameServerProviderServiceApplication : ApplicationBase
    {
        public GameServerProviderServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }
    }
}