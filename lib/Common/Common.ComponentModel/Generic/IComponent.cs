using System;

namespace Common.ComponentModel.Generic
{
    /// <inheritdoc />
    /// <summary>
    /// Represents a class which will be a component.
    /// </summary>
    /// <typeparam name="TOwner">An entity which owns this component.</typeparam>
    public interface IComponent<in TOwner> : IDisposable
        where TOwner : class
    {
        /// <summary>
        /// Awakes the component after it was added to the container of the component.
        /// </summary>
        /// <param name="owner">The entity owner.</param>
        /// <param name="components">The components provider.</param>
        void Awake(TOwner owner, IComponents components);
    }
}