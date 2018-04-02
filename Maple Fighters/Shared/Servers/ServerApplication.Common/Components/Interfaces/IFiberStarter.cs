using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components.Interfaces
{
    public interface IFiberStarter
    {
        IFiber GetFiberStarter();
    }
}