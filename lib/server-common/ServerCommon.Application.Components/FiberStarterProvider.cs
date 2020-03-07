using Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class FiberStarterProvider : ComponentBase, IFiberStarterProvider
    {
        private readonly IFiberProvider fiberProvider;

        public FiberStarterProvider(IFiberProvider fiberProvider)
        {
            this.fiberProvider = fiberProvider;
        }

        public IFiber ProvideFiberStarter()
        {
            var fiber = fiberProvider.GetFiber();
            fiber.Start();

            return fiber;
        }
    }
}