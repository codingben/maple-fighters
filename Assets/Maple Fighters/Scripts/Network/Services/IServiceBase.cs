using System;

namespace Scripts.Services
{
    public interface IServiceBase : IDisposable
    {
        IServiceConnectionHandler ServiceConnectionHandler { get; }
    }
}