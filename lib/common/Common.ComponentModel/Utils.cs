using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel
{
    internal static class Utils
    {
        public static void ThrowExceptionIfNotInterface<TComponent>()
            where TComponent : class
        {
            if (typeof(TComponent).IsInterface)
            {
                return;
            }

            throw new ComponentInterfaceExpectedException(typeof(TComponent).Name);
        }
    }
}