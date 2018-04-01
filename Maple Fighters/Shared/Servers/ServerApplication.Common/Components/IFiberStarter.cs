using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public interface IFiberStarter
    {
        IFiber GetFiberStarter();
    }
}