using System;

namespace Scripts.Services
{
    public interface IServiceBase : IPeerLogicBaseHandler, IDisposable
    {
        IServiceConnectionHandler ServiceConnectionHandler { get; }
    }
}