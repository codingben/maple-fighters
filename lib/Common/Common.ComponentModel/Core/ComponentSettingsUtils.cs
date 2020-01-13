using System;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    internal static class ComponentSettingsUtils
    {
        public static ComponentSettingsAttribute GetComponentSettings<TComponent>()
            where TComponent : class
        {
            var componentSettings =
                (ComponentSettingsAttribute)Attribute.GetCustomAttribute(
                    typeof(TComponent),
                    typeof(ComponentSettingsAttribute));
            if (componentSettings == null)
            {
                if (typeof(IDisposable).IsAssignableFrom(typeof(TComponent)))
                {
                    throw new ComponentSettingsMissingException(nameof(TComponent));
                }
            }

            return componentSettings;
        }
    }
}