using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    public interface IFiberStarterProvider
    {
        IFiber ProvideFiberStarter();
    }
}