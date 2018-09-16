using System;

namespace Common.ComponentModel.Exceptions
{
    public class InterfaceExpectedException<TComponent> : Exception
        where TComponent : class
    {
        public InterfaceExpectedException()
            : base(
                $"A communication between components by interfaces only. {typeof(TComponent).Name} is not interface.")
        {
            // Left blank intentionally
        }
    }
}