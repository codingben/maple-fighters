using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class FiberProvider : Component<IServerEntity>, IFiberStarter
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