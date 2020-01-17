using System;

namespace Common.ComponentModel.Core
{
    internal static class ComponentsContainerUtils
    {
        public static void SetComponentByLifetime<TComponent>(ref TComponent component)
            where TComponent : class
        {
            var componentSettings =
                ComponentSettingsUtils.GetComponentSettings<TComponent>();
            if (componentSettings != null)
            {
                var lifeTime = componentSettings.Lifetime;
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
                            (TComponent)Activator.CreateInstance(typeof(TComponent));
                        break;
                    }

                    default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public static ExposedState GetExposedState<TComponent>()
            where TComponent : class 
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