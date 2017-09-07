using System;
using ServerApplication.Common.ComponentModel;

namespace Shared.ServerApplication.Common.PeerLogic
{
    public interface IPeerEntity : IDisposable
    {
        IContainer Components { get; }
    }
}