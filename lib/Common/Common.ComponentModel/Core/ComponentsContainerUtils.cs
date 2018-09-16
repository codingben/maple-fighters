using System;
using Common.ComponentModel.Exceptions;

namespace Common.ComponentModel.Core
{
    public static class ComponentsContainerUtils
    {
        public static TComponent ProvideComponentByLifeTime<TComponent>(
            TComponent component)
            where TComponent : class
        {
            var lifeTime = GetLifeTime<TComponent>();

            switch (lifeTime)
            {
                case LifeTime.Singleton:
                {
                    break;
                }

                case LifeTime.PerThread:
                {
                    throw new NotImplementedException();
                }

                case LifeTime.PerCall:
                {
                    return (TComponent)Activator.CreateInstance(
                        typeof(TComponent));
                }
            }

            return component;
        }

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

        public static ExposedState GetExposedState<TComponent>()
            where TComponent : class 
        {
            var exposedState = ExposedState.Exposable;

            var componentSettings = GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                exposedState = componentSettings.ExposedState;
            }

            return exposedState;
        }

        public static LifeTime GetLifeTime<TComponent>()
            where TComponent : class 
        {
            var lifeTime = LifeTime.Singleton;

            var componentSettings = GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                lifeTime = componentSettings.LifeTime;
            }

            return lifeTime;
        }

        public static bool IsExposed<TComponent>()
            where TComponent : class 
        {
            var exposedState = GetExposedState<TComponent>();
            if (exposedState != ExposedState.Exposable)
            {
                throw new ComponentNotExposedException<TComponent>();
            }

            return true;
        }
    }
}