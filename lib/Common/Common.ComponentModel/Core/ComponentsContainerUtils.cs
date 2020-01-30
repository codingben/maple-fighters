using System;

namespace Common.ComponentModel.Core
{
    internal static class ComponentsContainerUtils
    {
        public static ExposedState GetExposedState<TComponent>()
            where TComponent : IComponent
        {
            var componentSettings =
                ComponentSettingsUtils.GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                return componentSettings.ExposedState;
            }

            throw new InvalidOperationException();
        }
    }
}