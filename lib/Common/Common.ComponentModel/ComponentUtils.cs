using System;

namespace Common.ComponentModel
{
    public class ComponentUtils
    {
        private static ComponentSettingsAttribute GetComponentSettings<TComponent>()
            where TComponent : class
        {
            var component = typeof(TComponent);

            var componentSettings = (ComponentSettingsAttribute)Attribute.GetCustomAttribute(
                component, typeof(ComponentSettingsAttribute));
            if (componentSettings == null)
            {
                if (IsComponent<TComponent>())
                {
                    throw new ComponentModelException(
                        $"A component {component.Name} should have component settings.");
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

        public static bool IsComponent<T>()
        {
            var component = typeof(T);
            return typeof(IDisposable).IsAssignableFrom(component);
        }

        public static bool IsInterface<T>()
        {
            if (typeof(T).IsInterface)
            {
                return true;
            }

            throw new ComponentModelException(
                $"Could not get a {typeof(T).Name} component via not interface type.");
        }

        public static bool IsExposed<TComponent>()
            where TComponent : class
        {
            if (IsComponent<TComponent>())
            {
                var exposedState = GetExposedState<TComponent>();
                if (exposedState != ExposedState.Exposable)
                {
                    throw new ComponentModelException(
                        $"A component {typeof(TComponent).Name} should be exposed.");
                }
            }

            return true;
        }
    }
}