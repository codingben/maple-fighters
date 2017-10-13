using Login.Application;
using ServerApplication.Common.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Login.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<LoginApplication>
    {
        protected override LoginApplication CreateApplication(IFiberProvider fiberProvider)
        {
            return new LoginApplication(fiberProvider);
        }
    }
}