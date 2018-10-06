using System;

namespace Common.ComponentModel
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a class which will be a component.
    /// </summary>
    public interface IComponent : IDisposable
    {
        void Awake(IComponents components);
    }
}