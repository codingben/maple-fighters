using System;

namespace Common.ComponentModel
{
    /// <summary>
    /// Represents a class that will be a component.
    /// </summary>
    public interface IComponent : IDisposable
    {
        void Awake(IComponents components);
    }
}