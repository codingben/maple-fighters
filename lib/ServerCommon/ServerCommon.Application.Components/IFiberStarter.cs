using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    public interface IFiberStarter
    {
        IFiber ProvideFiberStarter();
    }
}