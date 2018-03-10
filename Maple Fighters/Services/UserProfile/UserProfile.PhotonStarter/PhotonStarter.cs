using PhotonStarter.Common;
using ServerCommunicationInterfaces;
using UserProfile.Service.Application;

namespace UserProfile.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<UserProfileServiceApplication>
    {
        protected override UserProfileServiceApplication CreateApplication(IFiberProvider fiberProvider, IServerConnector serverConnector)
        {
            return new UserProfileServiceApplication(fiberProvider, serverConnector);
        }
    }
}