using System;

namespace ComponentModel.Common
{
    public interface IEntity : IDisposable
    {
        IContainer Components { get; }
    }

    public interface IEntity<out T> : IDisposable 
        where T : IEntity<T>
    {
        IContainer<T> Components { get; }
    }
}