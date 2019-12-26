using System;

namespace Common.ComponentModel.Core
{
    internal static class ComponentsContainerUtils
    {
        public static void SetComponentByLifetime<TComponent>(ref TComponent component)
            where TComponent : class
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var lifeTime = Lifetime.Singleton;
            var componentSettings =
                ComponentSettingsUtils.GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                lifeTime = componentSettings.Lifetime;
            }

            switch (lifeTime)
            {
                case Lifetime.Singleton:
                {
                    break;
                }

                case Lifetime.PerThread:
                {
                    throw new NotImplementedException();
                }

                case Lifetime.PerCall:
                {
                    component =
                        (TComponent)Activator.CreateInstance(
                            typeof(TComponent));
                    break;
                }
            }
        }

        public static ExposedState GetExposedState<TComponent>()
            where TComponent : class 
        {
            var exposedState = ExposedState.Exposable;

            var componentSettings =
                ComponentSettingsUtils.GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                exposedState = componentSettings.ExposedState;
            }

            return exposedState;
        }
    }
}