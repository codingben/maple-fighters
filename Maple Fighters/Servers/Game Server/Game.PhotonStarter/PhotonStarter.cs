using Game.Application;
using ServerApplication.Common.PhotonStarter;

namespace Game.PhotonStarter
{
    public class PhotonStarter : PhotonStarterBase<GameApplication>
    {
        protected override GameApplication CreateApplication()
        {
            return new GameApplication();
        }
    }
}