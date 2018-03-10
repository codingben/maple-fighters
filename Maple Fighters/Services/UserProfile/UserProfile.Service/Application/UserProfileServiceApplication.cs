using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace UserProfile.Service.Application
{
    public class UserProfileServiceApplication : ApplicationBase
    {
        public UserProfileServiceApplication(IFiberProvider fiberProvider, IServerConnector serverConnector) 
            : base(fiberProvider, serverConnector)
        {
            // Left blank intentionally
        }
    }
}