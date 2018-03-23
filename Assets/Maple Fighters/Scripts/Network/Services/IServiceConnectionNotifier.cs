using System;
using CommonCommunicationInterfaces;

namespace Scripts.Services
{
    public interface IServiceConnectionNotifier
    {
        event Action Connected;
        event Action<DisconnectReason, string> Disconnected;
    }
}