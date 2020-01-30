using System;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
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
                    throw new ComponentSettingsMissingException(nameof(TComponent));
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