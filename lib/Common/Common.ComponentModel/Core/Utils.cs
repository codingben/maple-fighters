using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    public static class Utils
    {
        public static void AssertNotInterface<TComponent>()
            where TComponent : class
        {
            if (typeof(TComponent).IsInterface)
            {
                return;
            }

            throw new InterfaceExpectedException<TComponent>();
        }
    }
}