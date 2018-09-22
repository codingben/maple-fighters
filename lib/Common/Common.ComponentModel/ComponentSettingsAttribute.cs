using System;

namespace Common.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentSettingsAttribute : Attribute
    {
        public ExposedState ExposedState { get; }

        public LifeTime LifeTime { get; }

        public ComponentSettingsAttribute(
            ExposedState exposedState,
            LifeTime lifeTime = LifeTime.Singleton)
        {
            ExposedState = exposedState;
            LifeTime = lifeTime;
        }
    }
}