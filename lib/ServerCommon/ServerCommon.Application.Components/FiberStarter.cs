using System;
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
            this.fiberProvider = fiberProvider
                                 ?? throw new ArgumentNullException(
                                     nameof(fiberProvider));
        }

        public IFiber GetFiberStarter()
        {
            fiberProvider.GetFiber().Start();
            return fiberProvider.GetFiber();
        }
    }
}