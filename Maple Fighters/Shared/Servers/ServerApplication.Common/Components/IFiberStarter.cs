using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public interface IFiberStarter : IExposableComponent
    {
        IFiber GetFiberStarter();
    }
}