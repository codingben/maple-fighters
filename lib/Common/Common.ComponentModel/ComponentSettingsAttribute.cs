using System;

namespace Common.ComponentModel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentSettingsAttribute : Attribute
    {
        public ExposedState ExposedState { get; }

        public ComponentSettingsAttribute(ExposedState exposedState)
        {
            ExposedState = exposedState;
        }
    }
}