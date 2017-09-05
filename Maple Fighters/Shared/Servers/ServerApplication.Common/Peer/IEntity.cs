using System;
using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public interface IEntity : IDisposable
    {
        int Id { get; }

        IContainer<EntityComponent> Components { get; }
    }
}