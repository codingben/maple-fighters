using ComponentModel.Common;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class FiberProvider : Component, IFiberStarter
    {
        private readonly IFiberProvider fiberProvider;

        public FiberProvider(IFiberProvider fiberProvider)
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