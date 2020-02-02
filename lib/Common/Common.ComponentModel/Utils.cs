using System;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel
{
    internal static class Utils
    {
        public static ComponentSettingsAttribute GetComponentSettings<TComponent>()
            where TComponent : IComponent
        {
            var componentSettings =
                (ComponentSettingsAttribute)Attribute.GetCustomAttribute(
                    typeof(TComponent),
                    typeof(ComponentSettingsAttribute));
            if (componentSettings == null)
            {
                if (typeof(IDisposable).IsAssignableFrom(typeof(TComponent)))
                {
                    throw new ComponentSettingsMissingException(typeof(TComponent).Name);
                }
            }

            return componentSettings;
        }

        public static ExposedState GetExposedState<TComponent>()
            where TComponent : IComponent
        {
            var componentSettings = GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                return componentSettings.ExposedState;
            }

            throw new InvalidOperationException();
        }

        public static void ThrowExceptionIfNotInterface<TComponent>()
            where TComponent : class
        {
            if (typeof(TComponent).IsInterface)
            {
                return;
            }

            throw new ComponentSettingsMissingException(typeof(TComponent).Name);
        }
    }
}