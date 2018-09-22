using Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerCommon.Application.Components
{
    [ComponentSettings(ExposedState.Unexposable)]
    public class FiberStarter : ComponentBase, IFiberStarter
    {
        private readonly IFiberProvider fiberProvider;

        public FiberStarter(IFiberProvider fiberProvider)
        {
            this.fiberProvider = fiberProvider;
        }

        public IFiber GetFiberStarter()
        {
            var fiber = fiberProvider.GetFiber();
            fiber.Start();
            return fiber;
        }
    }
}