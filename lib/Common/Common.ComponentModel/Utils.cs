using System;

namespace Common.ComponentModel
{
    public class Utils
    {
        public static ComponentSettingsAttribute GetComponentSettings<TComponent>()
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
                        $"A component {component.Name} should have a component settings attribute.");
                }
            }

            return componentSettings;
        }

        public static ExposedState GetComponentExposedState<TComponent>()
            where TComponent : class
        {
            var exposedState = ExposedState.Unexposable;

            var componentSettings = GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                exposedState = componentSettings.ExposedState;
            }

            return exposedState;
        }

        public static LifeTime GetComponentLifeTime<TComponent>()
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
                $"Could not get a {typeof(T).Name} via not interface type.");
        }
    }
}