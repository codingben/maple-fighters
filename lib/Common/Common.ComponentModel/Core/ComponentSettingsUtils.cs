using System;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    internal static class ComponentSettingsUtils
    {
        public static ComponentSettingsAttribute GetComponentSettings<TComponent>()
            where TComponent : class
        {
            var component = typeof(TComponent);

            var componentSettings =
                (ComponentSettingsAttribute)Attribute.GetCustomAttribute(
                    component,
                    typeof(ComponentSettingsAttribute));
            if (componentSettings == null)
            {
                if (typeof(IDisposable).IsAssignableFrom(component))
                {
                    throw new ComponentSettingsMissingException<TComponent>();
                }
            }

            return componentSettings;
        }
    }
}