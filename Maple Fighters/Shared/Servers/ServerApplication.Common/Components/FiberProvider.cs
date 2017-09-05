﻿using ServerApplication.Common.ComponentModel;
using ServerCommunicationInterfaces;

namespace ServerApplication.Common.Components
{
    public class FiberProvider : CommonComponent
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