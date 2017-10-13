using Chat.Application;
using ServerApplication.Common.PhotonStarter;
using ServerCommunicationInterfaces;

namespace Chat.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<ChatApplication>
    {
        protected override ChatApplication CreateApplication(IFiberProvider fiberProvider)
        {
            return new ChatApplication(fiberProvider);
        }
    }
}