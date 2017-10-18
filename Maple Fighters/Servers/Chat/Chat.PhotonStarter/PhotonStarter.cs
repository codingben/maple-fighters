using Chat.Application;
using PhotonStarter.Common;
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