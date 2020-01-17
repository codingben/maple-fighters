using System;

namespace ServerCommon.Application.Components
{
    public interface IClientPeerContainer
    {
        void Add(IDisposable peer);

        void Remove(int id);

        bool Get(int id, out IDisposable peer);
    }
}