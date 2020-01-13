using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    internal static class Utils
    {
        public static void AssertNotInterface<TComponent>()
            where TComponent : class
        {
            if (typeof(TComponent).IsInterface)
            {
                return;
            }

            throw new ComponentSettingsMissingException(nameof(TComponent));
        }
    }
}