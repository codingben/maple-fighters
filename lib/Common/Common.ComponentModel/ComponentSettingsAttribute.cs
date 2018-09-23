using System;

namespace Common.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentSettingsAttribute : Attribute
    {
        public ExposedState ExposedState { get; }

        public Lifetime Lifetime { get; }

        public ComponentSettingsAttribute(
            ExposedState exposedState,
            Lifetime lifeTime = Lifetime.Singleton)
        {
            ExposedState = exposedState;
            Lifetime = lifeTime;
        }
    }
}