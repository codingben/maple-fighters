using Game.Application;
using PhotonStarter.Common;
using ServerCommunicationInterfaces;

namespace Game.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<GameApplication>
    {
        protected override GameApplication CreateApplication(IFiberProvider fiberProvider)
        {
            return new GameApplication(fiberProvider);
        }
    }
}